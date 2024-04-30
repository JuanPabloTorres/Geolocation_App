namespace GeolocationAds.CustomContentView;

public class CustomEntryContentView : ContentView
{
    public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomEntryContentView), default(string), BindingMode.TwoWay);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty PlaceholderProperty =
           BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(CustomEntryContentView), default(string), BindingMode.TwoWay);

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public static readonly BindableProperty IsPasswordProperty =
          BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(CustomEntryContentView), default(bool), BindingMode.TwoWay);

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    public static readonly BindableProperty IsEnabledProperty =
       BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(CustomEntryContentView), default(bool), BindingMode.TwoWay);

    public bool IsEnabled
    {
        get => (bool)GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    public CustomEntryContentView()
    {
        var entry = new Entry
        {
            Keyboard = Keyboard.Text,
            Style = (Style)Application.Current.Resources["globalEntry"]
        };
        entry.SetBinding(Entry.TextProperty, new Binding(nameof(Text), source: this));

        entry.SetBinding(Entry.IsPasswordProperty, new Binding(nameof(IsPassword), source: this));

        entry.SetBinding(Entry.PlaceholderProperty, new Binding(nameof(Placeholder), source: this));

        entry.SetBinding(Entry.IsEnabledProperty, new Binding(nameof(IsEnabled), source: this));

        var frame = new Border
        {
            Style = (Style)Application.Current.Resources["CustomBorderStyle"],

            Content = new VerticalStackLayout
            {
                Children = { entry }
            }
        };

        Content = new VerticalStackLayout
        {
            Margin = 5,
            Children = { frame }
        };
    }
}