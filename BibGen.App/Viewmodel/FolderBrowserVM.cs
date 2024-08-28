using CommunityToolkit.Mvvm.ComponentModel;

namespace BibGen.App.Viewmodel
{
    public partial class FolderBrowserVM : ObservableObject
    {
        [ObservableProperty]
        private string _browseButtonContent = "Browse...";

        [ObservableProperty]
        private string _placeholderContent = "Select folder";

        [ObservableProperty]
        private string _folderPathContent = "";

        [ObservableProperty]
        private string _title = "";
    }
}
