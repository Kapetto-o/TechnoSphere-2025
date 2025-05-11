using System.Windows.Input;

namespace TechnoSphere_2025.managers
{
    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        public DelegateCommand(Action<T> execute) => _execute = execute;
        public bool CanExecute(object? parameter) => true;
        public void Execute(object? parameter) => _execute((T)parameter!);
        public event EventHandler? CanExecuteChanged;
    }
}
