namespace GeolocationAds.ViewTemplates;

public partial class VideoDisplayer : Grid
{
    public static readonly BindableProperty IsMyContentStackHeaderVisibleProperty = BindableProperty.Create(
          "IsMyContentStackHeaderVisible",
          typeof(bool),
          typeof(VideoDisplayer),
          defaultValue: false,
          propertyChanged: IsMyContentStackHeaderVisibleChanged);

    public bool IsMyContentStackHeaderVisible
    {
        get => (bool)GetValue(IsMyContentStackHeaderVisibleProperty);
        set => SetValue(IsMyContentStackHeaderVisibleProperty, value);
    }

    public static readonly BindableProperty IsSearchContentStackHeaderVisibleProperty = BindableProperty.Create(
          "IsSearchContentStackHeaderVisible",
          typeof(bool),
          typeof(VideoDisplayer),
          defaultValue: false,
          propertyChanged: IsSearchContentStackHeaderVisibleChanged);

    public bool IsSearchContentStackHeaderVisible
    {
        get => (bool)GetValue(IsSearchContentStackHeaderVisibleProperty);
        set => SetValue(IsSearchContentStackHeaderVisibleProperty, value);
    }

    public VideoDisplayer()
	{
		InitializeComponent();
	}

    private static void IsMyContentStackHeaderVisibleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = bindable as VideoDisplayer;

        if (view != null)
        {
            view.myContentStackHeader.IsVisible = (bool)newValue;

            if (view.myContentStackHeader.IsVisible)
            {
                view.searchContentStackHeader.IsVisible = false;

                view.searchContentStackHeaderBtn.IsVisible = false;
            }
        }
    }

    private static void IsSearchContentStackHeaderVisibleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = bindable as VideoDisplayer;

        if (view != null)
        {
            view.searchContentStackHeader.IsVisible = (bool)newValue;

            if (view.searchContentStackHeader.IsVisible)
            {
                view.searchContentStackHeaderBtn.IsVisible = true;

                view.myContentStackHeader.IsVisible = false;
            }
        }
    }
}