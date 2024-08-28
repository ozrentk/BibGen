using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace BibGen.App.Viewmodel
{
    public partial class StripeItemVM : ObservableObject
    {
        [ObservableProperty]
        private string _fontName = "";

        [ObservableProperty]
        private int _fontSize;

        [ObservableProperty]
        private Color _color;

        [ObservableProperty]
        private decimal _baseline;

        [ObservableProperty]
        private string _excelColumnName;

        [ObservableProperty]
        private bool _isSelected;
    }
}
