using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;

namespace SieveOfEratosthenes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// <remarks>
    /// Ideas for the UI taken from: https://github.com/goodface87/PrimeCalculator
    /// </remarks>
    public partial class MainWindow : Window
    {
        private BackgroundWorker worker = new BackgroundWorker();  // worker to execute the sieve on a different thread than the UI thread, idea from : http://www.wpf-tutorial.com/misc-controls/the-progressbar-control/
        private List<int> primeNumbers = new List<int>();          // initialize a list to hold all the prime numbers found

        public MainWindow()
        {
            InitializeComponent();

            // worker - subscribe to events and ensure the worker can give progress reports 
            worker.DoWork += Worker_SieveOfEratosthenes;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.WorkerReportsProgress = true;

            displayPrimes.ItemsSource = primeNumbers;    // bind the primeNumbers list to the listbox
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (worker.IsBusy) return;    // ensures the current input is fully processed before accepting more input
            primeNumbers.Clear();         
            int n;                        // initialize the int that will hold the input from the user
            if (int.TryParse(numberBox.Text, out n) && n >1)  // if the user enters an integer greater than 1
            {
                progressBar.Maximum = n;
                calcPrimesText.Text = "Calculating primes up to " + n.ToString() + "...";  // update the text box to let the users know that the algorithm is calculating 
                worker.RunWorkerAsync(n);    // start the worker on another thread
            }
            else MessageBox.Show("Error: Please enter an integer greater than 1.");         // if the user does not enter an integer greater than 1, show a simple error message
            numberBox.Clear();
            numberBox.Focus();   // ensures the user can always type in the numberBox without having to click on it
        }

        // The Sieve of Eratosthenes is implemented here. This method is based off of the pseudocode found at: https://en.wikipedia.org/wiki/Sieve_of_Eratosthenes#Algorithm_and_variants.
        // The worker executes this method in a separate thread, allowing the UI to function as normal while this thread runs. 
        private void Worker_SieveOfEratosthenes(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            int n = (int)e.Argument;                                           // take the number from the user and cast it to an int

            try {
                List<bool> markNumbers = Enumerable.Repeat(true, n + 1).ToList();  // create a new boolean list that is indexed from 0 to n and initialize every entry to true

                // start at the lowest prime (2) and check every number through root n
                for (int i = 2; i <= Math.Sqrt(n); i++)
                {
                    if (markNumbers[i])  // if the number being checked has not been set to false (i.e. "crossed out")
                    {
                        // starting with the square of the current number, set every multiple of the number to false (i.e. cross out every multiple of the current number)
                        for (int j = (int)Math.Pow(i, 2); j <= n; j += i) markNumbers[j] = false;
                    }
                }

                // find the indicies of markNumbers that contain a true value (i.e. all of the numbers that have not been "crossed out")
                primeNumbers.Add(2);   // 2 will always appear in the list

                // check the rest of the odd numbers to see if they are prime
                for (int i = 3; i < markNumbers.Count; i+=2)
                {
                    if (markNumbers[i])   // if the number is true (i.e. not "crossed out")
                    {
                        primeNumbers.Add(i);        // add the number to the list of primes
                        worker.ReportProgress(i);   // call the function to update the progress bar
                        Thread.Sleep(1);            // allow this thread to sleep so the UI can be updated
                    }
                }

                worker.ReportProgress(markNumbers.Count-1);  // fills the remaining portion of the progress bar

            } catch (OutOfMemoryException) {
                MessageBox.Show("Error: The number you chose was too large. Please choose a smaller number.");
            }
        }

        // called when the worker finishes executing
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            displayPrimes.Items.Refresh();                                                            // refresh the primes on the screen
            if (primeNumbers.Count != 0) displayPrimes.BorderThickness = new Thickness(0, 5, 0, 5);   // add a border around the box of primes

            // update the calculating textbox - takes care of both singluar and plural use of the word "prime"
            if (primeNumbers.Count == 1) calcPrimesText.Text = primeNumbers.Count.ToString() + " prime up to " + progressBar.Value.ToString();
            else calcPrimesText.Text = primeNumbers.Count.ToString() + " primes up to " + progressBar.Value.ToString();

            progressBar.Value = 0;   // reset the progress bar                                                                          
        }

        // update the progress bar
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }
    }
}
