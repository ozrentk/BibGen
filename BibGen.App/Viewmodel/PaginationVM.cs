using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Windows.Input;

namespace BibGen.App.Viewmodel
{
    public partial class PaginationVM : ObservableObject
    {
        [JsonIgnore]
        public ICommand FirstPageCommand { get; set; }
        [JsonIgnore]
        public ICommand PreviousPageCommand { get; set; }
        [JsonIgnore]
        public ICommand NextPageCommand { get; set; }
        [JsonIgnore]
        public ICommand LastPageCommand { get; set; }

        [ObservableProperty]
        private int _currentItem;

        [ObservableProperty]
        private int _itemCount;

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
