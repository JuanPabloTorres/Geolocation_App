using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace GeolocationAds.ViewTemplates;

public partial class PaginControls : Grid
{
    // Define public events for clicks
    //public event EventHandler BackClicked;

    //public event EventHandler NextClicked;

    public Action? OnBackClickedAction { get; set; }
    public Action? OnNextClickedAction { get; set; }

    public static readonly BindableProperty IsBackButtonEnabledProperty = BindableProperty.Create(
            "IsBackButtonEnabled",
            typeof(bool),
            typeof(PaginControls),
            defaultValue: true,
            propertyChanged: OnIsBackButtonEnabledChanged);

    public bool IsBackButtonEnabled
    {
        get => (bool)GetValue(IsBackButtonEnabledProperty);
        set => SetValue(IsBackButtonEnabledProperty, value);
    }

    public static readonly BindableProperty IsNextButtonEnabledProperty = BindableProperty.Create(
          "IsNextButtonEnabled",
          typeof(bool),
          typeof(PaginControls),
          defaultValue: true,
          propertyChanged: OnINextButtonEnabledChanged);

    public bool IsNextButtonEnabled
    {
        get => (bool)GetValue(IsNextButtonEnabledProperty);
        set => SetValue(IsNextButtonEnabledProperty, value);
    }

    public static readonly BindableProperty IsNextButtonVisibleProperty = BindableProperty.Create(
         "IsNextButtonVisible",
         typeof(bool),
         typeof(Button),
         defaultValue: true,
         propertyChanged: OnNextButtonVisibleChanged);

    public bool IsNextButtonVisible
    {
        get => (bool)GetValue(IsNextButtonVisibleProperty);
        set => SetValue(IsNextButtonVisibleProperty, value);
    }

    public static readonly BindableProperty IsBackButtonVisibleProperty = BindableProperty.Create(
      "IsBackButtonVisible",
      typeof(bool),
      typeof(Button),
      defaultValue: true,
      propertyChanged: OnBackButtonVisibleChanged);

    public bool IsBackButtonVisible
    {
        get => (bool)GetValue(IsBackButtonVisibleProperty);
        set => SetValue(IsBackButtonVisibleProperty, value);
    }

    public PaginControls()
    {
        InitializeComponent();
    }

    private void BackItemButton_Clicked(object sender, EventArgs e)
    {
        //BackClicked?.Invoke();

        OnBackClickedAction?.Invoke();
    }

    private void NextItemButton_Clicked(object sender, EventArgs e)
    {
        //NextClicked?.Invoke();

        OnNextClickedAction?.Invoke();
    }

    private static void OnIsBackButtonEnabledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = bindable as PaginControls;

        if (view != null)
        {
            view.BackBtn.IsEnabled = (bool)newValue;
        }
    }

    private static void OnINextButtonEnabledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = bindable as PaginControls;

        if (view != null)
        {
            view.NextBtn.IsEnabled = (bool)newValue;
        }
    }

    private static void OnNextButtonVisibleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = bindable as PaginControls;

        if (view != null)
        {
            view.NextBtn.IsVisible = (bool)newValue;
        }
    }

    private static void OnBackButtonVisibleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = bindable as PaginControls;

        if (view != null)
        {
            view.BackBtn.IsVisible = (bool)newValue;
        }
    }
}