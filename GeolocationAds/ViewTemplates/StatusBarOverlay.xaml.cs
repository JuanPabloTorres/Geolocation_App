namespace GeolocationAds.ViewTemplates;

public partial class StatusBarOverlay : ContentView
{
	public StatusBarOverlay()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty CollectionCountProperty =
       BindableProperty.Create(nameof(CollectionCount), typeof(int), typeof(StatusBarOverlay), 0);

    public static readonly BindableProperty AdTypeProperty =
        BindableProperty.Create(nameof(AdType), typeof(string), typeof(StatusBarOverlay), string.Empty);

    public static readonly BindableProperty DistanceProperty =
        BindableProperty.Create(nameof(Distance), typeof(string), typeof(StatusBarOverlay), string.Empty);

    public int CollectionCount
    {
        get => (int)GetValue(CollectionCountProperty);
        set => SetValue(CollectionCountProperty, value);
    }

    public string AdType
    {
        get => (string)GetValue(AdTypeProperty);
        set => SetValue(AdTypeProperty, value);
    }

    public string Distance
    {
        get => (string)GetValue(DistanceProperty);
        set => SetValue(DistanceProperty, value);
    }
}