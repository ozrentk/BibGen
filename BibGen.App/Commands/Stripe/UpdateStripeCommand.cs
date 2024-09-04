using BibGen.App.Viewmodel;
using System.Windows.Input;

namespace BibGen.App.Commands.Stripe
{
    public class UpdateStripeCommand : ICommand
    {
        private readonly MainWindowVM _mainWindowVm;

        public UpdateStripeCommand(MainWindowVM mainWindowVm)
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
            if (parameter is not StripePropertiesVM stripeProps)
                return;

            if (_mainWindowVm.SelectedStripeItem == null)
                return;

            _mainWindowVm.SelectedStripeItem.FontName = stripeProps.SelectedFont;
            _mainWindowVm.SelectedStripeItem.FontSize = stripeProps.FontSize;
            _mainWindowVm.SelectedStripeItem.Color = stripeProps.Color;
            _mainWindowVm.SelectedStripeItem.Baseline = stripeProps.Baseline;
            _mainWindowVm.SelectedStripeItem.ExcelColumnName = stripeProps.ExcelColumnName;
        }
    }
}
