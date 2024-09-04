using BibGen.App.Viewmodel;
using System.Windows;
using System.Windows.Input;

namespace BibGen.App.Commands.File
{
    public class NewBibProjectCommand : ICommand
    {
        private MainWindowVM _viewModel;

        public NewBibProjectCommand(MainWindowVM viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) =>
            true;

        public async void Execute(object parameter)
        {
            try
            {
                if (parameter is not MainWindow mainWindow)
                    return;

                mainWindow.DataContext = new MainWindowVM();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
