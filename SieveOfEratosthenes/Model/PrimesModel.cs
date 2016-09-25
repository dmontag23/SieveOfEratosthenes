using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SieveOfEratosthenes.Model
{
    /// <summary>
    /// Model connected to the PrimesViewModel
    /// </summary>
    public class PrimesModel
    {
        // The Sieve of Eratosthenes is implemented here. This method is based off of the pseudocode found at: https://en.wikipedia.org/wiki/Sieve_of_Eratosthenes#Algorithm_and_variants.
        public ObservableCollection<int> FindPrimeNumbers(int n)
        {
            List<bool> markNumbers = Enumerable.Repeat(true, n + 1).ToList();  // create a new boolean list that is indexed from 0 to n and initialize every entry to true
            ObservableCollection<int> primeNumbers = new ObservableCollection<int>();

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
            for (int i = 3; i < markNumbers.Count; i += 2)
            {
                if (markNumbers[i]) primeNumbers.Add(i);   // if the number is true (i.e. not "crossed out"), add the number to the list of primes
            }

            return primeNumbers;
        }
    }
}
