using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace App.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Func<object?, Task> execute;
        private readonly Func<object?, bool> canExecute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Func<object?, Task> execute, Func<object?, bool>? canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
            => canExecute?.Invoke(parameter) ?? true;

        public async void Execute(object? parameter)
            => await execute(parameter);

        public void RaiseCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
