using System.ComponentModel;

namespace SieveOfEratosthenes
{

    /// <summary>
    /// Implementation of the INotifyPropertyChanged Class
    /// </summary>
    public class MyINotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChangedEvent(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
