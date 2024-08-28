using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace BibGen.App.Viewmodel
{
    public partial class StripePropertiesVM : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ComboBoxItem> _fontItems;

        [ObservableProperty]
        private ObservableCollection<int> _fontSizes =
            new ObservableCollection<int> { 6, 7, 8, 9, 10, 11, 12, 14, 16, 18,
                        20, 22, 24, 26, 28, 36, 48, 60, 72, 84, 96, 120,
                        144, 168, 192, 216, 264, 312, 360, 408, 456, 552 };

        [ObservableProperty]
        public ComboBoxItem _selectedFont;

        [ObservableProperty]
        private int _fontSize = 12;

        [ObservableProperty]
        private Color _color = Color.FromRgb(0, 0, 0);

        [ObservableProperty]
        private decimal _baseline = 0.5M;

        [ObservableProperty]
        private string _excelColumnName;
    }
}
