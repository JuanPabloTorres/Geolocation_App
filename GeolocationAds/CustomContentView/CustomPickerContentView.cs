using System.Collections;

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

    public CustomPickerContentView()
    {
        var picker = new Picker
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
}