using System.Collections;
using ToolsLibrary.Extensions;

namespace GeolocationAds.CustomContentView;

public class CustomPickerContentView : ContentView
{
    public static readonly BindableProperty SelectedItemProperty =
           BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(CustomPickerContentView), default(object), BindingMode.TwoWay);

    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public static readonly BindableProperty DisplayItemProperty =
          BindableProperty.Create(nameof(DisplayItem), typeof(object), typeof(CustomPickerContentView), default(object), BindingMode.TwoWay);

    public object DisplayItem
    {
        get => GetValue(DisplayItemProperty);
        set => SetValue(DisplayItemProperty, value);
    }

    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(CustomPickerContentView), default(IEnumerable), BindingMode.OneWay);

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly BindableProperty TitleProperty =
           BindableProperty.Create(nameof(Title), typeof(string), typeof(CustomPickerContentView), default(string), BindingMode.TwoWay);

    public object Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    // Create a custom bindable property for ItemDisplayBindingPath
    public static readonly BindableProperty ItemDisplayBindingPathProperty =
        BindableProperty.Create(nameof(ItemDisplayBindingPath), typeof(string), typeof(CustomPickerContentView), default(string));

    public string ItemDisplayBindingPath
    {
        get => (string)GetValue(ItemDisplayBindingPathProperty);
        set => SetValue(ItemDisplayBindingPathProperty, value);
    }

    // Create a custom bindable property for ItemDisplayBindingPath
    public static readonly BindableProperty SelectValueProperty =
        BindableProperty.Create(nameof(SelectValue), typeof(string), typeof(CustomPickerContentView), default(string));

    public string SelectValue
    {
        get => (string)GetValue(SelectValueProperty);
        set => SetValue(SelectValueProperty, value);
    }

    private Picker picker;

    public CustomPickerContentView()
    {
        picker = new Picker
        {
            Style = (Style)Application.Current.Resources["globalPicker"]
        };

        picker.SetBinding(Picker.SelectedItemProperty, new Binding(nameof(SelectedItem), source: this));

        picker.SetBinding(Picker.ItemsSourceProperty, new Binding(nameof(ItemsSource), source: this));

        var frame = new Frame
        {
            Style = (Style)Application.Current.Resources["entryFrame"],
            Content = picker
        };

        Content = new StackLayout
        {
            Margin = 5,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            Orientation = StackOrientation.Vertical,
            Children = { frame }
        };
    }

    protected override void OnBindingContextChanged()
    {
        if (!string.IsNullOrEmpty(ItemDisplayBindingPath))
        {
            picker.ItemDisplayBinding = new Binding(ItemDisplayBindingPath);

            // Select the first item from the ItemsSource if it's not empty

            if (!DisplayItem.IsObjectNull())
            {
                SelectedItem = DisplayItem;
            }
            else
            {
                if (ItemsSource != null && ItemsSource.GetEnumerator().MoveNext())
                {
                    // Use LINQ to find the item with the matching "Value"
                    var matchingItem = ItemsSource?.Cast<object>().FirstOrDefault(item =>
                    {
                        // Replace "Value" with the name of the property you want to check
                        var valueProperty = item.GetType().GetProperty("Value");
                        if (valueProperty != null)
                        {
                            var itemValue = valueProperty.GetValue(item);
                            return itemValue != null && itemValue.ToString() == SelectValue;
                        }
                        return false;
                    });

                    SelectedItem = ItemsSource.Cast<object>().FirstOrDefault();
                }
            }
        }
    }
}