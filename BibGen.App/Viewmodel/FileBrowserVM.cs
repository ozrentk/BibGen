using CommunityToolkit.Mvvm.ComponentModel;

namespace BibGen.App.Viewmodel
{
    public partial class FileBrowserVM : ObservableObject
    {
        [ObservableProperty]
        private string _browseButtonContent = "Browse...";

        [ObservableProperty]
        private string _placeholderContent = "Select file";

        //[ObservableProperty]
        private string _filePathContent = "";
        // TODO: This is a workaround for the issue that the default value is not set in the XAML.
        // Should fix.
        public string FilePathContent
        {
            get => string.IsNullOrEmpty(_filePathContent) ?
                    _defaultFilePathContent :
                    _filePathContent;
            set => SetProperty(ref _filePathContent, value);
        }


        [ObservableProperty]
        private string _defaultFilePathContent = "";

        [ObservableProperty]
        private string _filter = "";

        [ObservableProperty]
        private string _title = "";
    }
}
