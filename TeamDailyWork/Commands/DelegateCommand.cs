using System;
using System.Windows.Input;

namespace TeamDailyWork.Commands
{
    public class DelegateCommand:ICommand
    {
        public bool CanExecute(object parameter)
        {
            if (CanExecuteFunc == null)
            {
                return true;
            }
            return CanExecuteFunc(parameter);
        }

        public void Execute(object parameter)
        {
            if (ExecuteAction == null)
            {
                return;
            }
            else
            {
                this.ExecuteAction(parameter);
            }
        }

        public event EventHandler CanExecuteChanged;
        public Action<Object> ExecuteAction { get; set; }
        public Func<Object, bool> CanExecuteFunc { get; set; }
    }
}
