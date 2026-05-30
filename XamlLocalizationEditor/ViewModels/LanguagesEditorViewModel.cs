using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using XamlLocalizationEditor.Models;

namespace XamlLocalizationEditor.ViewModels
{
    public partial class LanguagesEditorViewModel : ObservableObject
    {
        public LanguagesCollection Lang { get; }

        public ObservableCollection<LocalizationRow> Rows { get; } = new();

        public ObservableCollection<GridColumn> Columns { get; } = new();

        public LanguagesEditorViewModel(LanguagesCollection lang)
        {
            Lang = lang;

            Build();
        }

        public void Build()
        {
            Rows.Clear();
            Columns.Clear();

            var keys = Lang.Resources
                .SelectMany(r => r.Strings.Keys)
                .Distinct()
                .ToList();

            foreach (var key in keys)
            {
                var row = new LocalizationRow
                {
                    Key = key
                };

                foreach (var lang in Lang.Resources)
                {
                    row.Values[lang.LangKey.Name] =
                        lang.Strings.TryGetValue(key, out var val) ? val : "";
                }

                Rows.Add(row);
            }

            Columns.Add(new GridColumn
            {
                Header = "Key",
                BindingPath = "Key"
            });

            foreach (var lang in Lang.Resources)
            {
                Columns.Add(new GridColumn
                {
                    Header = lang.LangKey.Name,
                    BindingPath = $"Values[{lang.LangKey.Name}]"
                });
            }
        }
    }
}
