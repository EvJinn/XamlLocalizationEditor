using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using XamlLocalizationEditor.Models;

namespace XamlLocalizationEditor.Controls
{
    public static class DataGridColumnsBehavior
    {
        public static readonly DependencyProperty ColumnsSourceProperty =
            DependencyProperty.RegisterAttached(
                "ColumnsSource",
                typeof(IEnumerable<GridColumn>),
                typeof(DataGridColumnsBehavior),
                new PropertyMetadata(null, OnChanged));

        public static void SetColumnsSource(DependencyObject obj, IEnumerable<GridColumn> value)
            => obj.SetValue(ColumnsSourceProperty, value);

        public static IEnumerable<GridColumn> GetColumnsSource(DependencyObject obj)
            => (IEnumerable<GridColumn>)obj.GetValue(ColumnsSourceProperty);

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = (DataGrid)d;
            grid.Columns.Clear();

            if (e.NewValue is IEnumerable<GridColumn> cols)
            {
                foreach (var col in cols)
                {
                    var elementStyle = new Style(typeof(TextBlock));
                    elementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.WrapWithOverflow));

                    var editingStyle = new Style(typeof(TextBox));
                    editingStyle.Setters.Add(new Setter(TextBox.TextWrappingProperty, TextWrapping.WrapWithOverflow));
                    editingStyle.Setters.Add(new Setter(TextBox.AcceptsReturnProperty, true));

                    var textColumn = new DataGridTextColumn
                    {
                        Header = col.Header,
                        Binding = new Binding(col.BindingPath)
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        }
                    };

                    textColumn.ElementStyle = elementStyle;
                    textColumn.EditingElementStyle = editingStyle;

                    grid.Columns.Add(textColumn);
                }
            }
        }
    }
}
