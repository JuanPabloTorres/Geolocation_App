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

        image.SetBinding(Image.SourceProperty, new Binding(nameof(IconSource), source: this));

        //var frame = new Border
        //{
        //    Style = (Style)Application.Current.Resources["CustomBorderStyle"],

        //    Content = new VerticalStackLayout
        //    {

        //        Children = { entry }
        //    }
        //};

        //var frame = new Border
        //{
        //    Style = (Style)Application.Current.Resources["CustomBorderStyle"],
        //    HorizontalOptions = LayoutOptions.Fill,

        //    Content = new HorizontalStackLayout
        //    {
        //        Spacing = 5,
        //        HorizontalOptions = LayoutOptions.Fill,
        //        Children = { image, entry }
        //    }
        //};

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