using BibGen.App.Viewmodel;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Input;

namespace BibGen.App.UserControls
{
    public partial class FileBrowserUserControl : UserControl
    {
        public FileBrowserUserControl()
        {
            InitializeComponent();
        }

        private void OpenCmdCanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void OpenCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is not FileBrowserVM fileBrowserVM)
                return;

            var ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.CurrentDirectory;
            ofd.Filter = fileBrowserVM.Filter;
            ofd.Title = fileBrowserVM.Title;

            if (ofd.ShowDialog() == false)
                return;

            fileBrowserVM.FilePathContent = ofd.FileName;
        }
    }
}
