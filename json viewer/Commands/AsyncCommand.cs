using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace json_viewer.Commands
{
    /// <summary>
    /// Async Command
    /// 
    /// Based on John Thiriet AsynCommand
    /// Source : https://johnthiriet.com/mvvm-going-async-with-async-command/
    /// </summary>
    public interface IAsyncCommand<T> : ICommand
    {
        Task ExecuteAsync(T parameter);
        bool CanExecute(T parameter);
    }

    public class AsyncCommand<T> : IAsyncCommand<T>
    {
        private bool _isExecuting;
        private readonly Func<T, Task> _execute;
        private readonly Func<T, bool> _canExecute;

        public AsyncCommand(
            Func<T, Task> execute,
            Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(T parameter)
        {
            return !_isExecuting && (_canExecute?.Invoke(parameter) ?? true);
        }

        public async Task ExecuteAsync(T parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    _isExecuting = true;
                    await _execute(parameter);
                }
                finally
                {
                    _isExecuting = false;
                }
            }
            RaiseCanExecuteChanged();
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #region ICommand methods implementation
        public bool CanExecute(object parameter)
        {

            return CanExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            /// We ignore the warning, since this way of executing
            /// is the sync way
            ExecuteAsync((T)parameter);
        }
        #endregion
    }
}
