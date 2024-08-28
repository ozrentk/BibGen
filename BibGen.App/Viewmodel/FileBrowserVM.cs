using CommunityToolkit.Mvvm.ComponentModel;

namespace BibGen.App.Viewmodel
{
    public class FileBrowserVM : ObservableObject
    {
        private string _browseButtonContent = "Browse...";
        private string _placeholderContent = "Select file";
        private string _filePathContent = "";
        private string _defaultFilePathContent = "";
        private string _filter = "";
        private string _title = "";

        public string BrowseButtonContent
        {
            get => _browseButtonContent;
            set => SetProperty(ref _browseButtonContent, value);
        }

        public string PlaceholderContent
        {
            get => _placeholderContent;
            set => SetProperty(ref _placeholderContent, value);
        }

        public string FilePathContent
        {
            get => string.IsNullOrEmpty(_filePathContent) ? 
                    _defaultFilePathContent :
                    _filePathContent;
            set => SetProperty(ref _filePathContent, value);
        }

        public string DefaultFilePathContent
        {
            get => _defaultFilePathContent;
            set => SetProperty(ref _defaultFilePathContent, value);
        }

        public string Filter
        {
            get => _filter;
            set => SetProperty(ref _filter, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}
