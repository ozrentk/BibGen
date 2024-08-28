using BibGen.App.Viewmodel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BibGen.App.UserControls
{
    /// <summary>
    /// Interaction logic for StripeListViewUserControl.xaml
    /// </summary>
    public partial class StripeListViewUserControl : UserControl
    {
        private MainWindowVM MyDataContext =>
            (MainWindowVM)DataContext;

        public StripeListViewUserControl()
        {
            InitializeComponent();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.Escape)
            {
                StripeListView.SelectedItem = null;
            }
        }

        private void ListView_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (sender is not ListView listView)
            //    return;

            //listView.SelectedItem = null;
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item && 
                item.DataContext is StripeItemVM selectedStripe)
            {
                if (selectedStripe == null)
                    return;

                MyDataContext.SelectedStripeItem = selectedStripe;

                foreach(var stripeItem in MyDataContext.StripeItems)
                {
                    stripeItem.IsSelected =
                        stripeItem == selectedStripe;
                }

                MyDataContext.StripePropertiesVM.FontSize = selectedStripe.FontSize;
                MyDataContext.StripePropertiesVM.Baseline = selectedStripe.Baseline;
                MyDataContext.StripePropertiesVM.ExcelColumnName = selectedStripe.ExcelColumnName;

                var selectedFontItem = MyDataContext.StripePropertiesVM.FontItems.FirstOrDefault(x => x.Content.ToString() == selectedStripe.FontName);
                MyDataContext.StripePropertiesVM.SelectedFont = selectedFontItem;
            }
        }
    }
}
