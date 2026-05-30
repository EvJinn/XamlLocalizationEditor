using CommunityToolkit.Mvvm.ComponentModel;

namespace XamlLocalizationEditor.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty] private MenuViewModel _menuVM;
        [ObservableProperty] private EditorViewModel _editorVM;

        public MainViewModel(MenuViewModel menuVM, EditorViewModel editorVM)
        {
            MenuVM = menuVM;
            EditorVM = editorVM;
        }

    }
}
