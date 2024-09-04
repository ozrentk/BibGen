using BibGen.App.Viewmodel;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace BibGen.App.Commands.File
{
    public class OpenBibProjectCommand : ICommand
    {
        private MainWindowVM _viewModel;

        public OpenBibProjectCommand(MainWindowVM viewModel)
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

                var ofd = new OpenFileDialog();
                ofd.InitialDirectory = Environment.CurrentDirectory;
                ofd.Filter = "Bib project files (*.bibprj)|*.bibprj";
                ofd.Title = "Open Bib Project...";

                if (ofd.ShowDialog() == false)
                    return;

                string jsonString;
                using (var outputFile = new StreamReader(ofd.FileName))
                {
                    jsonString = await outputFile.ReadToEndAsync();
                }

                _viewModel = JsonConvert.DeserializeObject<MainWindowVM>(jsonString);
                _viewModel.SavedAsFilePath = ofd.FileName;

                mainWindow.DataContext = _viewModel;
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
