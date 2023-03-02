using Anton.Paar.Assignment.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Anton.Paar.Assignment.Commands
{
    public class CancelProcessCommand : ICommand
    {
        private readonly MainWindowViewModel _viewModel;

        public CancelProcessCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.CanCancelProcessing();
        }

        public void Execute(object parameter)
        {
            _viewModel.CancelProcessing();
            CommandManager.InvalidateRequerySuggested();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
