using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using XamlLocalizationEditor.Models;
using XamlLocalizationEditor.Services;

namespace XamlLocalizationEditor.ViewModels
{
    public partial class EditorViewModel : ObservableObject
    {
        private readonly AppState _appState;

        public List<LanguagesCollection> Langs => _appState.Items;

        [ObservableProperty]
        private object _selected;

        public EditorViewModel(AppState appState)
        {
            _appState = appState;

            _appState.ItemsChanged += OnItemsChanged;
        }


        [RelayCommand]
        private void SelectedItemChanged(object parameter)
        {
            if (parameter is LanguageResources)
            {
                Selected = (LanguageResources)parameter;
            }
            else if (parameter is LanguagesCollection lc)
            {
                Selected = new LanguagesEditorViewModel(lc);
            }
        }

        private void OnItemsChanged(object? sender, EventArgs e)
        {
            this.OnPropertyChanged(nameof(Langs));
        }
    }
}
