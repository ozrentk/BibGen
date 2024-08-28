using BibGen.App.Viewmodel;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BibGen.App.UserControls
{
    public partial class FolderBrowserUserControl : UserControl
    {
        public FolderBrowserUserControl()
        {
            InitializeComponent();
        }

        private void OpenCmdCanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void OpenCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is not FolderBrowserVM fileBrowserVM)
                return;

            var ofd = new OpenFolderDialog();
            ofd.InitialDirectory = Environment.CurrentDirectory;
            ofd.Title = fileBrowserVM.Title;

            if (ofd.ShowDialog() == false)
                return;

            fileBrowserVM.FolderPathContent = ofd.FolderName;
        }
    }
}
