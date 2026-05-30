using CommunityToolkit.Mvvm.ComponentModel;

namespace XamlLocalizationEditor.Models
{
    public class LocalizationRow : ObservableObject
    {
        public string Key { get; set; }

        public Dictionary<string, string> Values { get; set; } = new();

        public string this[string lang]
        {
            get => Values.TryGetValue(lang, out var v) ? v : "";
            set
            {
                Values[lang] = value;
                OnPropertyChanged(nameof(Values));
            }
        }
    }
}
