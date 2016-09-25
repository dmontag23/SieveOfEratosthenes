using SieveOfEratosthenes.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace SieveOfEratosthenes.ViewModel
{

    /// <summary>
    /// ViewModel for the PrimesView
    /// </summary>
    public class PrimesViewModel : MyINotifyPropertyChanged
    {
        private PrimesModel primesModel = new PrimesModel();       // used to connect the PrimesModel to the PrimesViewModel
        private BackgroundWorker worker = new BackgroundWorker();  // worker to execute the sieve on a different thread than the UI thread, idea from : http://www.wpf-tutorial.com/misc-controls/the-progressbar-control/

        // private internal representations of UI elements
        private int txtNumberBox = -1;    
        private string lblCalcPrimes;    
        private ObservableCollection<int> lstBxPrimeNumbers = new ObservableCollection<int>();

        // public representations of UI elements to be bound to by the UI

        public string TxtNumberBox
        {
            get { return txtNumberBox.ToString(); }
            set
            {
                if (value != string.Empty)
                {
                    int n;                                     // initialize the int that will hold the input from the user
                    if (int.TryParse(value, out n) && n > 1)   // ensure the user has entered an integer greater than 1
                    {
                        txtNumberBox = n;  
                        LblCalcPrimes = "";
                    }
                    else LblCalcPrimes = "Error. Please enter an integer greater than 1.";
                }
            }
        }

        public string LblCalcPrimes
        {
            get { return lblCalcPrimes; }
            set
            {
                lblCalcPrimes = value;
                RaisePropertyChangedEvent("LblCalcPrimes");
            }
        }

        public ObservableCollection<int> LstBxPrimeNumbers
        {
            get { return lstBxPrimeNumbers; }
            set
            {
                lstBxPrimeNumbers = value;
                RaisePropertyChangedEvent("LstBxPrimeNumbers");
            }
        }

        // Command bound to the BtnFindPrimes Button
        public ICommand BtnFindPrimesCommand
        { 
            get { return new MyICommand(FindPrimes); }
        }

        // helper method executed when BtnFindPrimes is clicked
        private void FindPrimes()
        {
            if (txtNumberBox != -1 && !worker.IsBusy)  // ensures the user input is not blank and the background worker is not already executing a task
            {
                worker.RunWorkerAsync();
                LblCalcPrimes = "Calculating primes up to " + txtNumberBox.ToString() + "...";
            }
        }

        // constructor - subscribes the background worker to different events
        public PrimesViewModel()
        {
            // worker - subscribe to events
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            LstBxPrimeNumbers = primesModel.FindPrimeNumbers(txtNumberBox);
        }

        // called when the worker finishes executing
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null) LblCalcPrimes = "Error: The number you entered is too large.";  // if any exception is thrown in the model (the only one being an OutOfMemory exception), tell the user to enter a smaller number
            else
            {
                if (LstBxPrimeNumbers.Count == 1) LblCalcPrimes = "There is " + LstBxPrimeNumbers.Count.ToString() + " prime between 2 and " + txtNumberBox.ToString() + ":";
                else LblCalcPrimes = "There are " + LstBxPrimeNumbers.Count.ToString() + " primes between 2 and " + txtNumberBox.ToString() + ":";
            }
            txtNumberBox = -1;
        }
    }
}
