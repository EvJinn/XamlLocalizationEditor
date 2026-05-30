using System.Globalization;

namespace XamlLocalizationEditor.Models
{
    public class LanguageResources
    {
        public readonly CultureInfo LangKey;
        public string Filename { get; private set; }
        public Dictionary<string, string> Strings { get; set; } = new();

        public LanguageResources(string langKey, string fileName)
        {
            LangKey = new CultureInfo(langKey);
            Filename = fileName;
        }

        public void SetFilename(string filename)
            => Filename = filename;

        public override string ToString()
            => LangKey.Name;
    }
}
