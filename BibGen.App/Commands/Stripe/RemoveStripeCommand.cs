using BibGen.App.Viewmodel;
using System.Windows.Input;

namespace BibGen.App.Commands.Stripe
{
    public class RemoveStripeCommand : ICommand
    {
        private readonly MainWindowVM _mainWindowVm;

        public RemoveStripeCommand(MainWindowVM mainWindowVm)
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

            _mainWindowVm.StripeItems.Remove(stripeItem);
        }
    }
}
