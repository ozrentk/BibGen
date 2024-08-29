using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;

namespace BibGen.App.Viewmodel
{
    public partial class StripePropertiesVM : ObservableObject
    {
        public ICommand LoadFontsCommand { get; set; }

        [ObservableProperty]
        private ObservableCollection<string> _fontItems;

        [ObservableProperty]
        private ObservableCollection<int> _fontSizes =
            new ObservableCollection<int> { 6, 7, 8, 9, 10, 11, 12, 14, 16, 18,
                        20, 22, 24, 26, 28, 36, 48, 60, 72, 84, 96, 120,
                        144, 168, 192, 216, 264, 312, 360, 408, 456, 552 };

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MandatoryPropertiesSelected))]
        public string _selectedFont;

        [ObservableProperty]
        private int _fontSize = 12;

        [ObservableProperty]
        private Color _color = Color.FromRgb(0, 0, 0);

        [ObservableProperty]
        private decimal _baseline = 0.5M;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MandatoryPropertiesSelected))]
        private string _excelColumnName;

        [ObservableProperty]
        private bool _mandatoryPropertiesSelected;

        public StripePropertiesVM()
        {
            LoadFontsCommand = new RelayCommand(LoadFonts);
        }

        private void LoadFonts()
        {
            var fonts = Fonts.SystemFontFamilies.Select(f => f.Source).OrderBy(x => x);
            FontItems = new ObservableCollection<string>(fonts);
        }

        partial void OnSelectedFontChanged(string value) =>
            CalculateMandatoryPropertiesSelected();

        partial void OnExcelColumnNameChanged(string value) =>
            CalculateMandatoryPropertiesSelected();

        private void CalculateMandatoryPropertiesSelected() =>
            MandatoryPropertiesSelected =
                !string.IsNullOrEmpty(SelectedFont) && 
                !string.IsNullOrEmpty(ExcelColumnName);
    }
}
