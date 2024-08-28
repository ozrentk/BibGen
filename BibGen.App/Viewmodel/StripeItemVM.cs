using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace BibGen.App.Viewmodel
{
    public class StripeItemVM : ObservableObject
    {
        private string _fontName = "";
        public string FontName
        {
            get => _fontName;
            set => SetProperty(ref _fontName, value);
        }

        private int _fontSize;
        public int FontSize
        {
            get => _fontSize;
            set => SetProperty(ref _fontSize, value);
        }

        private Color _color;
        public Color Color
        {
            get => _color;
            set => SetProperty(ref _color, value);
        }

        private decimal _baseline;
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

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }
}
