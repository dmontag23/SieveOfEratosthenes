using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            int numberFromUser;
            if (int.TryParse(numberBox.Text, out numberFromUser) && numberFromUser >1)  // if the user enters an integer greater than 1
            {
                List<int> primeNumbers = RunSieveOfEratosthenes(numberFromUser);        // find all the prime numbers less than or equal to the user's number
                displayPrimes.ItemsSource = primeNumbers;                               // display the primes in the listbox for the user
            }
            else  // if the user does not enter an integer greater than 1
            {
                MessageBox.Show("Error: Please enter an integer greater than 1");       // give the user a simple error message
                numberBox.Clear();
                numberBox.Focus();                                                      // ensures the user can type in the numberBox without having to click on it after receiving the error message
            }
        }

        /// <summary>
        /// Returns all prime numbers less than or equal to n. The input must be an integer greater than 1. 
        /// </summary>
        /// <remarks>
        /// This method is based off of the pseudocode found at: https://en.wikipedia.org/wiki/Sieve_of_Eratosthenes#Algorithm_and_variants
        /// </remarks>
        /// <param name="n"></param>
        private List<int> RunSieveOfEratosthenes(int n)
        {
            List<int> primeNumbers = new List<int>();                        // initialize the list to hold all the prime numbers less than or equal to n
            List<bool> markNumbers = Enumerable.Repeat(true, n+1).ToList();  // create a new boolean list that is indexed from 0 to n and initialized so every entry is true

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
                if (markNumbers[i]) primeNumbers.Add(i);
            }

            return primeNumbers;
        }
    }
}
