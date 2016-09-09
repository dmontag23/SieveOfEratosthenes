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
                worker.RunWorkerAsync(n);    // start the worker on another thread
            }
            else MessageBox.Show("Error: Please enter an integer greater than 1");       // if the user does not enter an integer greater than 1, show a simple error message

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
                List<bool> markNumbers = Enumerable.Repeat(true, n + 1).ToList();  // create a new boolean list that is indexed from 0 to n and initialized so every entry is true

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
                for (int i = 2; i < markNumbers.Count; i++)
                {
                    if (markNumbers[i])        // if the number is true (i.e. not "crossed out")
                    {
                        primeNumbers.Add(i);                             // add the number to the list of primes
                        worker.ReportProgress(i);                        // call the function to update the progress bar
                        Thread.Sleep(1);                                 // allow this thread to sleep so the UI can be updated
                    }
                }

                worker.ReportProgress(100);  // fills the remaining portion of the progress bar

            } catch (OutOfMemoryException) {
                MessageBox.Show("Oh no! The number you chose was too large. Please choose a smaller number.");
            }
        }

        // when the worker finishes executing, refresh the primes on the screen
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            displayPrimes.Items.Refresh();
        }

        // update the progress bar
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }
    }
}
