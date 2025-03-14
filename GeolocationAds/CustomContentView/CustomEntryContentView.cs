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

    public static readonly BindableProperty TitleProperty =
          BindableProperty.Create(nameof(Title), typeof(string), typeof(CustomEntryContentView), default(string), BindingMode.TwoWay);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly BindableProperty IconSourceProperty =
        BindableProperty.Create(nameof(IconSource), typeof(string), typeof(CustomEntryContentView), default(string), BindingMode.TwoWay);

    public string IconSource
    {
        get => (string)GetValue(IconSourceProperty);
        set => SetValue(IconSourceProperty, value);
    }

    public static readonly BindableProperty IconSizeProperty =
       BindableProperty.Create(nameof(IconSize), typeof(double), typeof(CustomEntryContentView), default(double), BindingMode.TwoWay);

    public double IconSize
    {
        get => (double)GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    // Evento TextChanged para permitir binding en XAML
    public event EventHandler<TextChangedEventArgs> TextChanged;

    private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomEntryContentView)bindable;
        control.TextChanged?.Invoke(control, new TextChangedEventArgs((string)oldValue, (string)newValue));
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

        // Evento TextChanged
        entry.TextChanged += (sender, args) =>
        {
            Text = args.NewTextValue; // Actualiza la propiedad `Text`
            TextChanged?.Invoke(this, args); // Dispara el evento
        };

        var titleLabel = new Label
        {
            VerticalOptions = LayoutOptions.Center,
            Style = (Style)Application.Current.Resources["globalsubTitle"]
        };

        titleLabel.SetBinding(Label.TextProperty, new Binding(nameof(Title), source: this));

        var image = new Image()
        {
            HeightRequest = 20,
            WidthRequest = 20,
            Margin = 4,
            Aspect = Aspect.AspectFit,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Start
        };

        // Set up the FontImageSource for the image
        var fontImageSource = new FontImageSource
        {
            FontFamily = "MaterialIcons-Regular",            
            
            Color = (Color)Application.Current.Resources["AppBlack"]
        };

        fontImageSource.SetBinding(FontImageSource.GlyphProperty, new Binding(nameof(IconSource), source: this));
       
        fontImageSource.SetBinding(FontImageSource.SizeProperty, new Binding(nameof(IconSize), source: this));

        // Assign the FontImageSource to the image
        image.Source = fontImageSource;

        // Create a new Grid
        var grid = new Grid
        {
            ColumnSpacing = 5,
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Star }
            },
            RowDefinitions = { new RowDefinition { Height = GridLength.Auto } }
        };

        grid.Add(image, 0, 0);

        grid.Add(entry, 1, 0);

        var frame = new Border
        {
            Style = (Style)Application.Current.Resources["CustomBorderStyle"],

            Content = grid
        };

        Content = new VerticalStackLayout
        {
            Margin = 5,
            Children = { frame }
        };
    }
}