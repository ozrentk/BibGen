using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace BibGen.App.Viewmodel
{
    public class PaginationVM : ObservableObject
    {
        private int _currentItem;
        public int CurrentItem
        {
            get => _currentItem;
            private set => SetProperty(ref _currentItem, value);
        }

        private int _itemCount;
        public int ItemCount
        {
            get => _itemCount;
            private set => SetProperty(ref _itemCount, value);
        }

        public ICommand FirstPageCommand { get; set; }
        public ICommand PreviousPageCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
        public ICommand LastPageCommand { get; set; }

        public PaginationVM()
        {
            FirstPageCommand = new RelayCommand(FirstPage);
            PreviousPageCommand = new RelayCommand(PreviousPage);
            NextPageCommand = new RelayCommand(NextPage);
            LastPageCommand = new RelayCommand(LastPage);
        }

        public void Reset(int count)
        {
            CurrentItem = 0;
            ItemCount = count;
        }

        private void FirstPage() =>
            CurrentItem = 0;

        private void PreviousPage()
        {
            if (CurrentItem <= 0)
                return;

            CurrentItem--;
        }

        private void NextPage()
        {
            if (CurrentItem + 1 >= ItemCount)
                return;

            CurrentItem++;
        }

        private void LastPage() => 
            CurrentItem = ItemCount - 1;
    }
}
