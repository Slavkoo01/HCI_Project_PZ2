using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NetworkService.ViewModel
{
    public class CommandBase : ICommand
    {
        //Fields

        private readonly Action<object> _execute;
        //used encapsulate a method that is to pass a method as a parameter

        private readonly Func<object, bool> _canExecute;
        //used to see if the action can be executed

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        //Constructor
        public CommandBase(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        //Constructor to execute action because not all commands need to be validated


        //Subscrieb or unsubscribe the Requery suggested event as needed
        //RequerySuggested events occure when CommandMannager detects if the command execution condition has changed


        //if the canExecuteAction field is null we return true since delegation predicate has not been established otherwise
        //we return the value of the delegate or the method that is going to be defined and delegated in the viewModel
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }
        //We just execute the action, same way this will execute method which will be delegated in viewModel
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
