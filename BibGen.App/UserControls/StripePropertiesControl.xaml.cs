using BibGen.App.Viewmodel;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace BibGen.App.UserControls
{
    /// <summary>
    /// Interaction logic for StripePropertiesControl.xaml
    /// </summary>
    public partial class StripePropertiesControl : UserControl
    {
        private StripePropertiesVM MyDataContext =>
            (StripePropertiesVM)DataContext;

        public static IEnumerable<string> FontNames =>
            Fonts.SystemFontFamilies.Select(x => $"{x}");

        public StripePropertiesControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var fonts = FontNames.Select(x => new ComboBoxItem { Content = x });
            MyDataContext.FontItems = new ObservableCollection<ComboBoxItem>(fonts);
        }
    }
}
