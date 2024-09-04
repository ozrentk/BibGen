using BibGen.App.Viewmodel;
using System.Windows.Input;

namespace BibGen.App.Commands.Stripe
{
    public class SelectStripeCommand : ICommand
    {
        private readonly MainWindowVM _mainWindowVm;

        public SelectStripeCommand(MainWindowVM mainWindowVm)
        {
            _mainWindowVm = mainWindowVm;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) =>
            true;

        public void Execute(object parameter)
        {
            if (parameter is not StripeItemVM stripeItem)
                return;

            if (stripeItem == null)
                return;

            _mainWindowVm.SelectedStripeItem = stripeItem;

            foreach (var eachStripeItem in _mainWindowVm.StripeItems)
            {
                eachStripeItem.IsSelected =
                    stripeItem == eachStripeItem;
            }

            _mainWindowVm.StripePropertiesVM.FontSize = stripeItem.FontSize;
            _mainWindowVm.StripePropertiesVM.Baseline = stripeItem.Baseline;
            _mainWindowVm.StripePropertiesVM.ExcelColumnName = stripeItem.ExcelColumnName;
            _mainWindowVm.StripePropertiesVM.Color = stripeItem.Color;

            var selectedFontItem = 
                _mainWindowVm.StripePropertiesVM.FontItems.FirstOrDefault(x => x.ToString() == stripeItem.FontName);
            _mainWindowVm.StripePropertiesVM.SelectedFont = selectedFontItem;

        }
    }
}
