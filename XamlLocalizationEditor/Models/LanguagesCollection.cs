using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;

namespace XamlLocalizationEditor.Models
{
    public partial class LanguagesCollection : ObservableObject
    {
        public readonly string Filepath;
        public string Filename => Path.GetFileName(Filepath);

        [ObservableProperty]
        public partial ObservableCollection<LanguageResources> Resources { get; set; } = new();

        public LanguagesCollection(string filePath)
        {
            Filepath = filePath;
        }

        public override string ToString()
            => Filename;
    }
}
