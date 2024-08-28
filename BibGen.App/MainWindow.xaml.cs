using BibGen.App.Viewmodel;
using BibGen.Services;
using BibGen.Svc.Model;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace BibGen.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowVM MyDataContext =>
            (MainWindowVM)DataContext;

        public MainWindow()
        {
            InitializeComponent();
            MyDataContext.ExcelFilePathVM.PropertyChanged += ExcelFilePathVM_PropertyChanged;
        }

        private void ExcelFilePathVM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FileBrowserVM.FilePathContent))
            {
                var fileBrowserViewModel = (FileBrowserVM)sender;
                if (File.Exists(fileBrowserViewModel.FilePathContent))
                {
                    var bibDataLoader = new BibDataLoader();
                    var bibPropertyNames = bibDataLoader.GetBibPropertyNames(fileBrowserViewModel.FilePathContent);
                    MyDataContext.BibPropertyNames = new ObservableCollection<string>(bibPropertyNames);

                    var bibData = bibDataLoader.LoadBibEntries(fileBrowserViewModel.FilePathContent);
                    MyDataContext.BibEntries = new ObservableCollection<BibEntry>(bibData);
                    MyDataContext.PaginationVM.Reset(bibData.Count);
                }
            }
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

        private void AddStripeCanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void AddStripeExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if(e.Parameter is not StripePropertiesVM stripeProps)
                return;

            MyDataContext.StripeItems.Add(new StripeItemVM
            {
                FontName = stripeProps.SelectedFont.Content.ToString(),
                FontSize = stripeProps.FontSize,
                Color = stripeProps.Color,
                Baseline = stripeProps.Baseline,
                ExcelColumnName = stripeProps.ExcelColumnName
            });
        }

        private void UpdateStripeCanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void UpdateStripeExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is not StripePropertiesVM stripeProps)
                return;

            if (MyDataContext.SelectedStripeItem == null)
                return;

            MyDataContext.SelectedStripeItem.FontName = stripeProps.SelectedFont.Content.ToString();
            MyDataContext.SelectedStripeItem.FontSize = stripeProps.FontSize;
            MyDataContext.SelectedStripeItem.Color = stripeProps.Color;
            MyDataContext.SelectedStripeItem.Baseline = stripeProps.Baseline;
            MyDataContext.SelectedStripeItem.ExcelColumnName = stripeProps.ExcelColumnName;
        }

        private void RemoveStripeCanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void RemoveStripeExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is StripeItemVM stripe)
            {
                MyDataContext.StripeItems.Remove(stripe);
            }
        }

        private void SelectStripeCanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void SelectStripeExecuted(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void DeselectStripeCanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void DeselectStripeExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            //BibBackgroundImage.Focus();
        }
    }
}