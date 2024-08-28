using BibGen.Svc.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Input;

namespace BibGen.App.Viewmodel
{
    public partial class MainWindowVM : ObservableObject
    {
        public ICommand ExportCurrentBibCommand { get; set; }
        public ICommand ExportAllBibsCommand { get; set; }

        [ObservableProperty]
        private FileBrowserVM _backgroundImageVM;

        [ObservableProperty]
        public FileBrowserVM _excelFilePathVM;

        [ObservableProperty]
        private FileBrowserVM _outputFolderVM;

        [ObservableProperty]
        private StripePropertiesVM _stripePropertiesVM;

        [ObservableProperty]
        private ObservableCollection<StripeItemVM> _stripeItems;

        [ObservableProperty]
        private StripeItemVM _selectedStripeItem;

        [ObservableProperty]
        private PaginationVM _paginationVM;

        [ObservableProperty]
        private ObservableCollection<string> _bibPropertyNames;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BibEntriesCount))]
        private ObservableCollection<BibEntry> _bibEntries;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(StatusMessage))]
        private int _bibEntriesCount;

        [ObservableProperty]
        private string _statusMessage;

        public MainWindowVM()
        {
            _excelFilePathVM = new FileBrowserVM
            {
                BrowseButtonContent = "Browse...",
                PlaceholderContent = "Select Excel file",
                DefaultFilePathContent = "pack://application:,,,/BibGen.App;component/ExcelFiles/example.xlsx",
                Filter = "Excel files (*.xlsx)|*.xlsx",
                Title = "Select Excel file"
            };

            _backgroundImageVM = new FileBrowserVM
            {
                BrowseButtonContent = "Browse...",
                PlaceholderContent = "Select background image file",
                DefaultFilePathContent = "pack://application:,,,/BibGen.App;component/Images/q.jpeg",
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                Title = "Select background image file"
            };

            _outputFolderVM = new FileBrowserVM
            {
                BrowseButtonContent = "Browse...",
                PlaceholderContent = "Select output folder",
                DefaultFilePathContent = "",
                Title = "Select output folder"
            };

            _stripePropertiesVM = new();
            _stripeItems = new();
            _bibEntries = new();
            _paginationVM = new();

            _bibEntries.CollectionChanged += BibEntries_CollectionChanged;

            ExportCurrentBibCommand = new RelayCommand(ExportCurrentBib);
            ExportAllBibsCommand = new RelayCommand(ExportAllBibs);
        }

        private void BibEntries_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) =>
            BibEntriesCount = _bibEntries.Count;

        partial void OnBibEntriesCountChanged(int value)
        {
            UpdateStatusMessage();
            PaginationVM.Reset(value);
        }

        private void UpdateStatusMessage() =>
            StatusMessage =
                _bibEntriesCount > 0 ?
                    $"Loaded {_bibEntriesCount} entries" :
                    "No entries loaded";

        public void ExportCurrentBib()
        {
            Debug.WriteLine($"Exporting current bib {PaginationVM.CurrentItem}");
        }

        public void ExportAllBibs()
        {
            Debug.WriteLine("Exporting all bibs");
        }
    }
}
