using System;
using System.Windows.Input;

namespace SieveOfEratosthenes
{

    /// <summary>
    /// Simple implementation of the ICommand Class
    /// </summary>
    public class MyICommand : ICommand
    {
        private Action action;

        public MyICommand(Action Action)
        {
            action = Action;
        }

        public void Execute(object parameter)
        {
            action();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
