using BibGen.App.Viewmodel;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace BibGen.App.Commands.File
{
    public class SaveBibProjectCommand : ICommand
    {
        private readonly MainWindowVM _viewModel;

        public SaveBibProjectCommand(MainWindowVM viewModel)
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
                if (string.IsNullOrEmpty(_viewModel.SavedAsFilePath))
                {
                    var sfd = new SaveFileDialog();
                    sfd.InitialDirectory = Environment.CurrentDirectory;
                    sfd.Filter = "Bib project files (*.bibprj)|*.bibprj";
                    sfd.Title = "Save Bib Project As...";

                    if (sfd.ShowDialog() == false)
                        return;

                    _viewModel.SavedAsFilePath = sfd.FileName;
                }
                string jsonString = JsonConvert.SerializeObject(_viewModel);

                using (var outputFile = new StreamWriter(_viewModel.SavedAsFilePath))
                {
                    await outputFile.WriteAsync(jsonString);
                }
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
