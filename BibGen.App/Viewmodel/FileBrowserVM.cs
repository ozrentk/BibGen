using CommunityToolkit.Mvvm.ComponentModel;

namespace BibGen.App.Viewmodel
{
    public partial class FileBrowserVM : ObservableObject
    {
        [ObservableProperty]
        private string _browseButtonContent = "Browse...";

        [ObservableProperty]
        private string _placeholderContent = "Select file";

        [ObservableProperty]
        private string _filePathContent = "";

        [ObservableProperty]
        private string _filter = "";

        [ObservableProperty]
        private string _title = "";
    }
}
