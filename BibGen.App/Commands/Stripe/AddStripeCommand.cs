using BibGen.App.Viewmodel;
using System.Windows.Input;

namespace BibGen.App.Commands.Stripe
{
    public class AddStripeCommand : ICommand
    {
        private readonly MainWindowVM _mainWindowVm;

        public AddStripeCommand(MainWindowVM mainWindowVm)
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

            _mainWindowVm.StripeItems.Add(new StripeItemVM
            {
                FontName = stripeProps.SelectedFont,
                FontSize = stripeProps.FontSize,
                Color = stripeProps.Color,
                Baseline = stripeProps.Baseline,
                ExcelColumnName = stripeProps.ExcelColumnName
            });
        }
    }
}
