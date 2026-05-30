using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.OpenFile;
using System.IO;
using XamlLocalizationEditor.Services;
using XamlLocalizationEditor.Tools;

namespace XamlLocalizationEditor.ViewModels
{
    public partial class MenuViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        private readonly AppState _appState;

        public MenuViewModel(IDialogService dialogService, AppState appState)
        {
            _dialogService = dialogService;
            _appState = appState;
        }

        [RelayCommand]
        private async Task OpenResourcesCollection()
        {
            string solutionFile = "";

            var od = new OpenFileDialogSettings()
            {
                DefaultExt = ".sln",
                Filter = "Visual studio projects|*.sln;*.slnx"
            };

            bool? success = _dialogService.ShowOpenFileDialog(this, od);
            if (success == true)
            {
                solutionFile = od.FileName;
            }

            if (string.IsNullOrEmpty(solutionFile)) return;

            var ext = Path.GetExtension(solutionFile);
            if (ext.ToLower() != ".sln" && ext.ToLower() != ".slnx")
            {
                _dialogService.ShowMessageBox(this, "Wrong file selected");
                return;
            }

            var f = FileIO.FindAllSupportedXamlFiles(Path.GetDirectoryName(solutionFile));

            if (f.Length < 1)
            {
                _dialogService.ShowMessageBox(this, "No supported files in solution");
                return;
            }

            try
            {
                _appState.OpenFromXamlFilesCollections(f);
            }
            catch (Exception e)
            {
                _dialogService.ShowMessageBox(this, e.Message);
            }
        }

        [RelayCommand]
        private async Task OpenTxtFile()
        {
            string txtFilename = "";

            var od = new OpenFileDialogSettings()
            {
                Filter = "Text file|*.txt"
            };

            bool? success = _dialogService.ShowOpenFileDialog(this, od);
            if (success == true)
            {
                txtFilename = od.FileName;
            }

            if (string.IsNullOrEmpty(txtFilename)) return;

            var ext = Path.GetExtension(txtFilename);
            if (ext.ToLower() != ".txt")
            {
                _dialogService.ShowMessageBox(this, "Wrong file selected");
                return;
            }

            try
            {
                var lang = FileIO.LoadFromTxt(txtFilename);
                if (lang != null)
                    _appState.Open(lang);
            }
            catch (Exception e)
            {
                _dialogService.ShowMessageBox(this, e.Message);
            }
        }

        [RelayCommand]
        private async Task SaveResourcesToXaml()
        {
            if (_appState.Items == null || _appState.Items.Count < 1 || _appState.CurrentCollection == null) return;

            foreach (var lang in _appState.CurrentCollection.Resources)
                FileIO.SaveXaml(lang, _appState.CurrentCollection.Filepath);
        }
    }
}
