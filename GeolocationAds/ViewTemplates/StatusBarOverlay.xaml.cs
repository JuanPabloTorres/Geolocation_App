namespace GeolocationAds.ViewTemplates;

public partial class StatusBarOverlay : ContentView
{
	public StatusBarOverlay()
	{
		InitializeComponent();
	}

    // Count Property
    public static readonly BindableProperty CollectionCountProperty =
        BindableProperty.Create(
            nameof(CollectionCount),
            typeof(int),
            typeof(StatusBarOverlay),
            0,
            propertyChanged: (bindable, _, _) => ((StatusBarOverlay)bindable).OnPropertyChanged(nameof(FormattedCount))
        );

    public int CollectionCount
    {
        get => (int)GetValue(CollectionCountProperty);
        set => SetValue(CollectionCountProperty, value);
    }

    // AdType Property
    public static readonly BindableProperty AdTypeProperty =
        BindableProperty.Create(nameof(AdType), typeof(string), typeof(StatusBarOverlay), string.Empty);

    public string AdType
    {
        get => (string)GetValue(AdTypeProperty);
        set => SetValue(AdTypeProperty, value);
    }

    // Distance Property
    public static readonly BindableProperty DistanceProperty =
        BindableProperty.Create(
            nameof(Distance),
            typeof(string),
            typeof(StatusBarOverlay),
            string.Empty,
            propertyChanged: (bindable, _, _) => ((StatusBarOverlay)bindable).OnPropertyChanged(nameof(FormattedDistance))
        );

    public string Distance
    {
        get => (string)GetValue(DistanceProperty);
        set => SetValue(DistanceProperty, value);
    }

    // Count Format Property
    public static readonly BindableProperty CountFormatProperty =
        BindableProperty.Create(nameof(CountFormat), typeof(string), typeof(StatusBarOverlay), "Nearby: {0:N0}");

    public string CountFormat
    {
        get => (string)GetValue(CountFormatProperty);
        set => SetValue(CountFormatProperty, value);
    }

    // Distance Format Property
    public static readonly BindableProperty DistanceFormatProperty =
        BindableProperty.Create(nameof(DistanceFormat), typeof(string), typeof(StatusBarOverlay), "Distance: {0}");

    public string DistanceFormat
    {
        get => (string)GetValue(DistanceFormatProperty);
        set => SetValue(DistanceFormatProperty, value);
    }

    // Formatted Properties
    public string FormattedCount => string.Format(CountFormat, CollectionCount);
    public string FormattedDistance => string.Format(DistanceFormat, Distance);


    public static readonly BindableProperty ShowCountProperty =
      BindableProperty.Create(nameof(ShowCount), typeof(bool), typeof(StatusBarOverlay), true);

    public bool ShowCount
    {
        get => (bool)GetValue(ShowCountProperty);
        set => SetValue(ShowCountProperty, value);
    }

    public static readonly BindableProperty ShowAdTypeProperty =
        BindableProperty.Create(nameof(ShowAdType), typeof(bool), typeof(StatusBarOverlay), true);

    public bool ShowAdType
    {
        get => (bool)GetValue(ShowAdTypeProperty);
        set => SetValue(ShowAdTypeProperty, value);
    }

    public static readonly BindableProperty ShowDistanceProperty =
        BindableProperty.Create(nameof(ShowDistance), typeof(bool), typeof(StatusBarOverlay), true);

    public bool ShowDistance
    {
        get => (bool)GetValue(ShowDistanceProperty);
        set => SetValue(ShowDistanceProperty, value);
    }

}