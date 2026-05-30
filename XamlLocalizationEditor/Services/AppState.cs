using System.IO;
using XamlLocalizationEditor.Models;
using XamlLocalizationEditor.Tools;

namespace XamlLocalizationEditor.Services
{
    public class AppState
    {
        private List<LanguagesCollection> _items = new();
        public List<LanguagesCollection> Items => _items.ToList();

        public LanguagesCollection CurrentCollection { get; private set; }

        public bool OpenFromXamlFilesCollections(string[] filenames)
        {
            if (filenames.Length < 1) return false;

            var path = Path.GetDirectoryName(filenames[0]);

            LanguagesCollection lang = new(path);

            foreach (var file in filenames)
            {
                var item = FileIO.OpenXamlResources(file);

                if (item == null) return false;

                lang.Resources.Add(item);   
            }

            _items.Add(lang);

            ItemsChanged?.Invoke(this, EventArgs.Empty);

            return true;
        }

        public bool Open(LanguagesCollection lang)
        {
            if (lang == null || lang.Resources.Count < 1) return false;

            _items.Add(lang);

            ItemsChanged?.Invoke(this, EventArgs.Empty);

            return true;
        }

        public bool Close(LanguagesCollection lang)
        {
            if (_items.Count < 0) return false;
            if (!_items.Contains(lang)) return false;

            _items.Remove(lang);

            return true;
        }

        public bool SetCurrent(LanguagesCollection lang)
        {
            if (_items.Count < 0) return false;
            if (!_items.Contains(lang)) return false;

            CurrentCollection = lang;
            CurrentChanged?.Invoke(this, EventArgs.Empty);

            return true;
        }

        public EventHandler? ItemsChanged;
        public EventHandler? CurrentChanged;
    }
}
