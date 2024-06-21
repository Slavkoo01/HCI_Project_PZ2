using System.Collections;
using System.Windows;
using System.Windows.Controls;

public static class ListViewSelectedItemsBehavior
{
    public static readonly DependencyProperty SelectedItemsProperty =
        DependencyProperty.RegisterAttached("SelectedItems",
            typeof(IList), typeof(ListViewSelectedItemsBehavior),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemsChanged));

    public static IList GetSelectedItems(DependencyObject obj)
    {
        return (IList)obj.GetValue(SelectedItemsProperty);
    }

    public static void SetSelectedItems(DependencyObject obj, IList value)
    {
        obj.SetValue(SelectedItemsProperty, value);
    }

    private static void OnSelectedItemsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        if (obj is ListView listView)
        {
            listView.SelectionChanged += (sender, args) =>
            {
                SetSelectedItems(listView, listView.SelectedItems);
            };
        }
    }
}
