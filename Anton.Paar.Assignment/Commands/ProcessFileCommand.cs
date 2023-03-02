using Anton.Paar.Assignment.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Anton.Paar.Assignment.Commands
{
    public class ProcessFileCommand : ICommand
    {
        private readonly MainWindowViewModel _viewModel;

        public ProcessFileCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.CanProcessFile();
        }

        public void Execute(object parameter)
        {
            _viewModel.ProcessFile();
            CommandManager.InvalidateRequerySuggested();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
