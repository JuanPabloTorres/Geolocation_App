using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace GeolocationAds.ViewTemplates;

public partial class PaginControls : StackLayout
{
    // Define public events for clicks
    public event EventHandler BackClicked;

    public event EventHandler NextClicked;

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

    public PaginControls()
    {
        InitializeComponent();
    }

    private void BackItemButton_Clicked(object sender, EventArgs e)
    {
        BackClicked?.Invoke(this, e);
    }

    private void NextItemButton_Clicked(object sender, EventArgs e)
    {
        NextClicked?.Invoke(this, e);
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
}