using BibGen.App.Viewmodel;
using BibGen.Svc.Model;
using System.Windows.Input;

namespace BibGen.App.Commands.File
{
    public class ExportAllBibsCommand : ExportBibBaseCommand, ICommand
    {
        private readonly MainWindowVM _viewModel;

        public ExportAllBibsCommand(MainWindowVM viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) =>
            _viewModel.IsExportAllowed;

        public void Execute(object parameter)
        {
            var stripeItems = _viewModel.StripeItems.Select(s => new BibLineDescriptor
            {
                FontName = s.FontName,
                FontSize = s.FontSize,
                Color = s.Color.ToString(),
                Baseline = (float)s.Baseline,
                ExcelColumnName = s.ExcelColumnName
            }).ToList();

            StartExportPipeline(
                _viewModel.ExcelFilePathVM.FilePathContent,
                _viewModel.BackgroundImageVM.FilePathContent,
                _viewModel.OutputFolderVM.FolderPathContent,
                stripeItems);
        }
    }
}
