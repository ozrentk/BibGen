using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;

namespace BibGen.App.Viewmodel
{
    public partial class StripePropertiesVM : ObservableObject
    {
        [JsonIgnore]
        public ICommand LoadFontsCommand { get; set; }

        [ObservableProperty]
        private ObservableCollection<string> _fontItems;

        [ObservableProperty]
        private ObservableCollection<int> _fontSizes =
            new ObservableCollection<int> { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100, 105, 110, 115, 120, 125, 130, 135, 140, 145, 150 };

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
