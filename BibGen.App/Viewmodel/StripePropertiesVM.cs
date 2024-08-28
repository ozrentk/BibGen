using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace BibGen.App.Viewmodel
{
    public class StripePropertiesVM : ObservableObject
    {
        private ObservableCollection<ComboBoxItem> _fontItems;

        public ObservableCollection<ComboBoxItem> FontItems
        {
            get => _fontItems;
            set => SetProperty(ref _fontItems, value);
        }

        private ObservableCollection<int> _fontSizes =
            new ObservableCollection<int> { 6, 7, 8, 9, 10, 11, 12, 14, 16, 18,
                        20, 22, 24, 26, 28, 36, 48, 60, 72, 84, 96, 120,
                        144, 168, 192, 216, 264, 312, 360, 408, 456, 552 };

        public ObservableCollection<int> FontSizes
        {
            get => _fontSizes;
            set => SetProperty(ref _fontSizes, value);
        }

        public ComboBoxItem _selectedFont;
        public ComboBoxItem SelectedFont
        {
            get => _selectedFont;
            set => SetProperty(ref _selectedFont, value);
        }

        private int _fontSize = 12;
        public int FontSize
        {
            get => _fontSize;
            set => SetProperty(ref _fontSize, value);
        }

        private Color _color = Color.FromRgb(0, 0, 0);
        public Color Color
        {
            get => _color;
            set => SetProperty(ref _color, value);
        }

        private decimal _baseline = 0.5M;
        public decimal Baseline
        {
            get => _baseline;
            set => SetProperty(ref _baseline, value);
        }

        private string _excelColumnName;
        public string ExcelColumnName
        {
            get => _excelColumnName;
            set => SetProperty(ref _excelColumnName, value);
        }
    }
}
