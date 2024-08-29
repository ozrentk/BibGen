using BibGen.Services;
using BibGen.Svc.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace BibGen.App.Viewmodel
{
    public partial class MainWindowVM : ObservableObject
    {
        public readonly BibDataLoader _bibDataLoader;
        public ICommand ExportCurrentBibCommand { get; set; }
        public ICommand ExportAllBibsCommand { get; set; }
        public ICommand AddStripeCommand { get; set; }
        public ICommand UpdateStripeCommand { get; set; }
        public ICommand RemoveStripeCommand { get; set; }
        public ICommand SelectStripeCommand { get; set; }

        [ObservableProperty]
        private FileBrowserVM _backgroundImageVM;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsExportAllowed))]
        public FileBrowserVM _excelFilePathVM;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsExportAllowed))]
        private FolderBrowserVM _outputFolderVM;

        [ObservableProperty]
        private StripePropertiesVM _stripePropertiesVM;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsExportAllowed))]
        private ObservableCollection<StripeItemVM> _stripeItems;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsStripeItemSelected))]
        private StripeItemVM _selectedStripeItem;

        [ObservableProperty]
        private bool _isStripeItemSelected;

        [ObservableProperty]
        private PaginationVM _paginationVM;

        [ObservableProperty]
        private ObservableCollection<string> _bibPropertyNames;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BibEntriesCount))]
        private ObservableCollection<BibEntry> _bibEntries;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(StatusMessage))]
        [NotifyPropertyChangedFor(nameof(BibEntriesLoaded))]
        [NotifyPropertyChangedFor(nameof(IsExportAllowed))]
        private int _bibEntriesCount;

        [ObservableProperty]
        private bool _bibEntriesLoaded;

        [ObservableProperty]
        private string _statusMessage;

        [ObservableProperty]
        private bool _isExportAllowed;

        public MainWindowVM()
        {
            _bibDataLoader = new(); // TODO: use DI container

            _excelFilePathVM = new FileBrowserVM
            {
                BrowseButtonContent = "Browse...",
                PlaceholderContent = "Select Excel file",
                FilePathContent = "",
                Filter = "Excel files (*.xlsx)|*.xlsx",
                Title = "Select Excel file"
            };

            _backgroundImageVM = new FileBrowserVM
            {
                BrowseButtonContent = "Browse...",
                PlaceholderContent = "Select background image file",
                FilePathContent = "",
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                Title = "Select background image file",
            };

            _outputFolderVM = new FolderBrowserVM
            {
                BrowseButtonContent = "Browse...",
                PlaceholderContent = "Select output folder",
                Title = "Select output folder",
                FolderPathContent = "",
            };

            _stripePropertiesVM = new();
            _stripeItems = new();
            _bibEntries = new();
            _paginationVM = new();

            _bibEntries.CollectionChanged += BibEntries_CollectionChanged;
            _excelFilePathVM.PropertyChanged += ExcelFilePathVM_PropertyChanged;
            _stripeItems.CollectionChanged += StripeItems_CollectionChanged;

            ExportCurrentBibCommand = new RelayCommand(ExportCurrentBib);
            ExportAllBibsCommand = new RelayCommand(ExportAllBibs);
            AddStripeCommand = new RelayCommand<StripePropertiesVM>(AddStripe);
            UpdateStripeCommand = new RelayCommand<StripePropertiesVM>(UpdateStripe);
            RemoveStripeCommand = new RelayCommand<StripeItemVM>(RemoveStripe);
            SelectStripeCommand = new RelayCommand<StripeItemVM>(SelectStripe);
        }

        private void BibEntries_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) =>
            BibEntriesCount = _bibEntries.Count;

        private void StripeItems_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) =>
            UpdateIsExportAllowedFlag();

        partial void OnSelectedStripeItemChanged(StripeItemVM value) =>
            IsStripeItemSelected = true;

        partial void OnBackgroundImageVMChanged(FileBrowserVM value) =>
            UpdateIsExportAllowedFlag();

        partial void OnOutputFolderVMChanged(FolderBrowserVM value) =>
            UpdateIsExportAllowedFlag();

        partial void OnStripeItemsChanged(ObservableCollection<StripeItemVM> value) =>
            UpdateIsExportAllowedFlag();

        partial void OnBibEntriesCountChanged(int value)
        {
            UpdateStatusMessage();
            UpdateLoadedFlag();
            UpdateIsExportAllowedFlag();
            PaginationVM.Reset(value);
        }

        private void ExcelFilePathVM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(FileBrowserVM.FilePathContent))
                return;
            
            var fileBrowserViewModel = (FileBrowserVM)sender;

            if (!File.Exists(fileBrowserViewModel.FilePathContent))
                return;

            var bibPropertyNames = _bibDataLoader.GetBibPropertyNames(fileBrowserViewModel.FilePathContent);
            BibPropertyNames = new ObservableCollection<string>(bibPropertyNames);

            var bibEntries = _bibDataLoader.LoadBibEntries(fileBrowserViewModel.FilePathContent);
            BibEntries.Clear();
            foreach (var bibEntry in bibEntries)
            {
                BibEntries.Add(bibEntry);
            }
        }

        private void UpdateLoadedFlag() =>
            BibEntriesLoaded = _bibEntriesCount > 0;

        private void UpdateStatusMessage() =>
            StatusMessage =
                _bibEntriesCount > 0 ?
                    $"Loaded {_bibEntriesCount} entries" :
                    "No entries loaded";

        private void UpdateIsExportAllowedFlag() =>
            IsExportAllowed =
                _bibEntriesCount > 0 &&
                !string.IsNullOrEmpty(BackgroundImageVM.FilePathContent) &&
                !string.IsNullOrEmpty(OutputFolderVM.FolderPathContent) &&
                StripeItems.Count > 0;

        public void AddStripe(StripePropertiesVM stripeProps) =>
            StripeItems.Add(new StripeItemVM
            {
                FontName = stripeProps.SelectedFont.ToString(),
                FontSize = stripeProps.FontSize,
                Color = stripeProps.Color,
                Baseline = stripeProps.Baseline,
                ExcelColumnName = stripeProps.ExcelColumnName
            });

        public void UpdateStripe(StripePropertiesVM stripeProps)
        {
            if (SelectedStripeItem == null)
                return;

            SelectedStripeItem.FontName = stripeProps.SelectedFont.ToString();
            SelectedStripeItem.FontSize = stripeProps.FontSize;
            SelectedStripeItem.Color = stripeProps.Color;
            SelectedStripeItem.Baseline = stripeProps.Baseline;
            SelectedStripeItem.ExcelColumnName = stripeProps.ExcelColumnName;
        }

        public void RemoveStripe(StripeItemVM stripeItem) =>
            StripeItems.Remove(stripeItem);

        public void SelectStripe(StripeItemVM stripeItem)
        {
            if (stripeItem == null)
                return;

            SelectedStripeItem = stripeItem;

            foreach (var eachStripeItem in StripeItems)
            {
                eachStripeItem.IsSelected =
                    stripeItem == eachStripeItem;
            }

            StripePropertiesVM.FontSize = stripeItem.FontSize;
            StripePropertiesVM.Baseline = stripeItem.Baseline;
            StripePropertiesVM.ExcelColumnName = stripeItem.ExcelColumnName;
            StripePropertiesVM.Color = stripeItem.Color;

            var selectedFontItem = StripePropertiesVM.FontItems.FirstOrDefault(x => x.ToString() == stripeItem.FontName);
            StripePropertiesVM.SelectedFont = selectedFontItem;
        }

        public void ExportCurrentBib() =>
            StartExportPipeline(PaginationVM.CurrentItem);

        public void ExportAllBibs() =>
            StartExportPipeline();

        private void StartExportPipeline(int? exportBibAt = null)
        {
            var pipeline = new BibPipeline(new BibDataLoader(), new BibImageGenerator(), new PdfGenerator());

            var stripeItems = StripeItems.Select(s => new BibLineDescriptor
            {
                FontName = s.FontName,
                FontSize = s.FontSize,
                Color = s.Color.ToString(),
                Baseline = (float)s.Baseline,
                ExcelColumnName = s.ExcelColumnName
            }).ToList();

            pipeline.GenerateBibs(new BibGenContext
            {
                ExcelFilePath = ExcelFilePathVM.FilePathContent,
                BackgroundFilePath = BackgroundImageVM.FilePathContent,
                OutputFilePath = OutputFolderVM.FolderPathContent,
                LineDescriptors = stripeItems,
                ExportBibAt = exportBibAt
            });

            MessageBox.Show(
                "Export completed", 
                "Export", 
                MessageBoxButton.OK, 
                MessageBoxImage.Information);
        }
    }
}
