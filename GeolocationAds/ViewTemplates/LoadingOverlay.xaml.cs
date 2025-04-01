namespace GeolocationAds.ViewTemplates;

public partial class LoadingOverlay : ContentView
{
	public LoadingOverlay()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty IsVisibleProperty = BindableProperty.Create(
       nameof(IsVisible),
       typeof(bool),
       typeof(LoadingOverlay),
       false,
       BindingMode.TwoWay);

    public new bool IsVisible
    {
        get => (bool)GetValue(IsVisibleProperty);
        set => SetValue(IsVisibleProperty, value);
    }
}