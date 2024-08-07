namespace GeolocationAds.ViewTemplates;

public partial class WebViewDisplayer : Grid
{
    public static readonly BindableProperty IsMyCaptureHeaderVisibleProperty = BindableProperty.Create(
        "IsMyCaptureHeaderVisible",
        typeof(bool),
        typeof(WebViewDisplayer),
        defaultValue: false,
        propertyChanged: IsMyCaptureHeaderVisibleChanged);

    public bool IsMyCaptureHeaderVisible
    {
        get => (bool)GetValue(IsMyCaptureHeaderVisibleProperty);
        set => SetValue(IsMyCaptureHeaderVisibleProperty, value);
    }

    public static readonly BindableProperty IsMyContentStackHeaderVisibleProperty = BindableProperty.Create(
        "IsMyContentStackHeaderVisible",
        typeof(bool),
        typeof(WebViewDisplayer),
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

    public WebViewDisplayer()
    {
        InitializeComponent();
    }

    private static void IsSearchContentStackHeaderVisibleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = bindable as WebViewDisplayer;

        if (view != null)
        {
            view.searchContentStackHeader.IsVisible = (bool)newValue;

            if (view.searchContentStackHeader.IsVisible)
            {
                view.myCaptureHeader.IsVisible = false;

                view.myContentStackHeader.IsVisible = false;
            }
        }
    }

    private static void IsMyContentStackHeaderVisibleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = bindable as WebViewDisplayer;

        if (view != null)
        {
            view.myContentStackHeader.IsVisible = (bool)newValue;

            if (view.myContentStackHeader.IsVisible)
            {
                view.searchContentStackHeader.IsVisible = false;

                view.myCaptureHeader.IsVisible = false;
            }
        }
    }

    private static void IsMyCaptureHeaderVisibleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = bindable as WebViewDisplayer;

        if (view != null)
        {
            view.myCaptureHeader.IsVisible = (bool)newValue;

            if (view.myCaptureHeader.IsVisible)
            {
                view.searchContentStackHeader.IsVisible = false;

                view.myContentStackHeader.IsVisible = false;
            }
        }
    }
}