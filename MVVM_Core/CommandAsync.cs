using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM_Core
{
    public class CommandAsync : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; _canExecteChanged = value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private event EventHandler _canExecteChanged;

        private Func<object, Task> _execute;
        private Func<object, bool> _canExeute;

        public CommandAsync(Func<object, Task> execute, Func<object, bool> canExeute = null)
        {
            _execute = execute;
            _canExeute = canExeute;
        }

        public bool CanExecute(object parameter)
        {
            return !_executing && (_canExeute == null || _canExeute(parameter));
        }

        bool _executing;

        public async void Execute(object parameter)
        {
            _executing = true;
            await _execute(parameter);
            _executing = false;
            _canExecteChanged?.Invoke(this, new EventArgs());
        }
    }
}
