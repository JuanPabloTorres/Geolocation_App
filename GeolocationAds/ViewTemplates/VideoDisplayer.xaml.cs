using System.Windows.Input;
using ToolsLibrary.Tools;

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

                //view.searchContentStackHeaderBtn.IsVisible = false;
            }
        }
    }

    public static readonly BindableProperty MediaOpenedCommandProperty = BindableProperty.Create(
        "MediaOpenedCommand",
        typeof(ICommand),
        typeof(VideoDisplayer),
        default(ICommand));

    public ICommand MediaOpenedCommand
    {
        get => (ICommand)GetValue(MediaOpenedCommandProperty);
        set => SetValue(MediaOpenedCommandProperty, value);
    }

    public static readonly BindableProperty SeekCompletedCommandProperty = BindableProperty.Create(
      "SeekCompletedCommand",
      typeof(ICommand),
      typeof(VideoDisplayer),
      default(ICommand));

    public ICommand SeekCompletedCommand
    {
        get => (ICommand)GetValue(SeekCompletedCommandProperty);
        set => SetValue(SeekCompletedCommandProperty, value);
    }

    private static void IsSearchContentStackHeaderVisibleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = bindable as VideoDisplayer;

        if (view != null)
        {
            view.searchContentStackHeader.IsVisible = (bool)newValue;

            if (view.searchContentStackHeader.IsVisible)
            {
                //view.searchContentStackHeaderBtn.IsVisible = true;

                view.myContentStackHeader.IsVisible = false;
            }
        }
    }

    private void videoPlayer_MediaEnded(object sender, EventArgs e)
    {
    }

    private void videoPlayer_SeekCompleted(object sender, EventArgs e)
    {
    }

    private async void videoPlayer_MediaFailed(object sender, CommunityToolkit.Maui.Core.Primitives.MediaFailedEventArgs e)
    {
        await CommonsTool.DisplayAlert("Error", e.ErrorMessage);
    }
}