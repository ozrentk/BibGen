using BibGen.App.Viewmodel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;

namespace BibGen.App.UserControls
{
    public partial class FileBrowserUserControl : UserControl
    {
        public FileBrowserUserControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(FileBrowserVM),
                typeof(FileBrowserUserControl),
                new PropertyMetadata(null));

        public FileBrowserVM ViewModel
        {
            get => (FileBrowserVM)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }
    }
}
