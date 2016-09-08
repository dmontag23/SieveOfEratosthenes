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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            int value;
            if (int.TryParse(numberBox.Text, out value) && value >1)
            {
                List<int> primeNumbers = RunSieveOfEratosthenes(value);
                displayPrimes.ItemsSource = primeNumbers;
            }
            else
            {
                MessageBox.Show("Please enter an integer greater than 1");
                numberBox.Clear();
                numberBox.Focus();
            }
        }

        private List<int> RunSieveOfEratosthenes(int n)
        {
            List<int> primeNumbers = new List<int>();
            List<bool> markNumbers = Enumerable.Repeat(true, n+1).ToList();  // create a new list that is indexed from 0 to n and initialized so every entry is true
            for (int i = 2; i <= Math.Sqrt(n); i++)
            {
                if (markNumbers[i])
                {
                    for (int j = (int)Math.Pow(i,2); j <= n; j += i) markNumbers[j] = false;
                }
            }

            for (int i = 2; i < markNumbers.Count; i++)
            {
                if (markNumbers[i]) primeNumbers.Add(i);
            }

            return primeNumbers;
        }
    }
}
