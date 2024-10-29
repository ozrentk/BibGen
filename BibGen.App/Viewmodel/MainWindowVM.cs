using BibGen.App.Commands.File;
using BibGen.App.Commands.Stripe;
using BibGen.Services;
using BibGen.Svc.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using SkiaSharp;
using SkiaSharp.Views.WPF;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;

namespace BibGen.App.Viewmodel
{
    public partial class MainWindowVM : ObservableObject
    {
        private readonly BibDataLoader _bibDataLoader;
        private readonly BibImageGenerator _bibImageGenerator;

        #region Commands
        [JsonIgnore]
        public ICommand ExportCurrentBibCommand { get; set; }
        [JsonIgnore]
        public ICommand ExportAllBibsCommand { get; set; }
        [JsonIgnore]
        public ICommand AddStripeCommand { get; set; }
        [JsonIgnore]
        public ICommand UpdateStripeCommand { get; set; }
        [JsonIgnore]
        public ICommand RemoveStripeCommand { get; set; }
        [JsonIgnore]
        public ICommand SelectStripeCommand { get; set; }
        [JsonIgnore]
        public ICommand NewBibProjectCommand { get; set; }
        [JsonIgnore]
        public ICommand OpenBibProjectCommand { get; set; }
        [JsonIgnore]
        public ICommand SaveBibProjectAsCommand { get; set; }
        [JsonIgnore]
        public ICommand SaveBibProjectCommand { get; set; }
        [JsonIgnore]
        public ICommand CloseBibProjectCommand { get; set; }
        [JsonIgnore]
        public ICommand ExitCommand { get; set; }
        #endregion Commands

        #region Observables
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

        [ObservableProperty]
        private string _savedAsFilePath;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BackgroundImageVM))]
        private ImageSource _previewImage;

        #endregion Observables

        public MainWindowVM()
        {
            _bibDataLoader = new(); // TODO: use DI container
            _bibImageGenerator = new(); // TODO: use DI container

            ExcelFilePathVM = new FileBrowserVM
            {
                BrowseButtonContent = "Browse...",
                PlaceholderContent = "Select Excel file",
                FilePathContent = "",
                Filter = "Excel files (*.xlsx)|*.xlsx",
                Title = "Select Excel file"
            };

            BackgroundImageVM = new FileBrowserVM
            {
                BrowseButtonContent = "Browse...",
                PlaceholderContent = "Select background image file",
                FilePathContent = "",
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                Title = "Select background image file",
            };

            OutputFolderVM = new FolderBrowserVM
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
            SubscribeToStripeItemPropertyChanged(_stripeItems);
            _backgroundImageVM.PropertyChanged += BackgroundImageVM_PropertyChanged;
            _paginationVM.PropertyChanged += PaginationVM_PropertyChanged;

            ExportCurrentBibCommand = new ExportCurrentBibCommand(this);
            ExportAllBibsCommand = new ExportAllBibsCommand(this);
            AddStripeCommand = new AddStripeCommand(this);
            UpdateStripeCommand = new UpdateStripeCommand(this);
            RemoveStripeCommand = new RemoveStripeCommand(this);
            SelectStripeCommand = new SelectStripeCommand(this);
            NewBibProjectCommand = new NewBibProjectCommand(this);
            OpenBibProjectCommand = new OpenBibProjectCommand(this);
            SaveBibProjectAsCommand = new SaveBibProjectAsCommand(this);
            SaveBibProjectCommand = new SaveBibProjectCommand(this);
            CloseBibProjectCommand = new CloseBibProjectCommand(this);
            ExitCommand = new ExitCommand(this);
        }

        private void SubscribeToStripeItemPropertyChanged(IEnumerable<StripeItemVM> items)
        {
            foreach (var item in items)
            {
                item.PropertyChanged += StripeItem_PropertyChanged;
            }
        }
        private void StripeItems_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (StripeItemVM newItem in e.NewItems)
                {
                    newItem.PropertyChanged += StripeItem_PropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (StripeItemVM oldItem in e.OldItems)
                {
                    oldItem.PropertyChanged -= StripeItem_PropertyChanged;
                }
            }

            UpdateIsExportAllowedFlag();
        }

        private void StripeItem_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(StripeItemVM.FontName) &&
                e.PropertyName != nameof(StripeItemVM.FontSize) &&
                e.PropertyName != nameof(StripeItemVM.Color) &&
                e.PropertyName != nameof(StripeItemVM.Baseline) &&
                e.PropertyName != nameof(StripeItemVM.ExcelColumnName))
                return;

            UpdatePreviewImageSource();
        }

        private void BibEntries_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) =>
            BibEntriesCount = _bibEntries.Count;

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

        private void BackgroundImageVM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(BackgroundImageVM.FilePathContent))
                return;

            UpdatePreviewImageSource();
        }

        private void PaginationVM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(PaginationVM.CurrentItem))
                return;

            UpdatePreviewImageSource();
        }


        private void UpdatePreviewImageSource()
        {
            if (BibEntries != null && !BibEntries.Any())
                return;

            if (BackgroundImageVM != null && string.IsNullOrWhiteSpace(BackgroundImageVM.FilePathContent))
                return;

            var itemIdx = PaginationVM.CurrentItem;
            var bibEntry = BibEntries[itemIdx];

            var background = SKBitmap.Decode(BackgroundImageVM.FilePathContent);
            var descriptors = StripeItems.Select(s => new BibLineDescriptor
            {
                FontName = s.FontName,
                FontSize = s.FontSize,
                Color = s.Color.ToString(),
                Baseline = (float)s.Baseline,
                ExcelColumnName = s.ExcelColumnName
            }).ToList();
            var skImage = _bibImageGenerator.GenerateBibImage(bibEntry, descriptors, background);
            var wBmp = skImage.ToWriteableBitmap();

            PreviewImage = wBmp;
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
    }
}
