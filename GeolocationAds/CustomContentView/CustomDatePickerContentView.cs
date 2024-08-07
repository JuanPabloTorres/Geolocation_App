namespace GeolocationAds.CustomContentView;

public class CustomDatePickerContentView : ContentView
{
    public static readonly BindableProperty SelectedDateProperty =
           BindableProperty.Create(nameof(SelectedDate), typeof(DateTime), typeof(CustomDatePickerContentView), default(DateTime), BindingMode.TwoWay);

    public DateTime SelectedDate
    {
        get => (DateTime)GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    public static readonly BindableProperty PlaceholderProperty =
       BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(CustomDatePickerContentView), default(string), BindingMode.TwoWay);

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public CustomDatePickerContentView()
    {
        var datePicker = new DatePicker
        {
            Date = SelectedDate,
            Format = "yyyy-MM-dd",
            Style = (Style)Application.Current.Resources["ModernDatePickerStyle"]
        };
        datePicker.SetBinding(DatePicker.DateProperty, new Binding(nameof(SelectedDate), source: this));

        var frame = new Border
        {
            Style = (Style)Application.Current.Resources["entryFrame"],

            Content = new VerticalStackLayout
            {
                Children = { datePicker }
            }
        };

        Content = new VerticalStackLayout
        {
            Margin = 5,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            Children = { frame }
        };
    }
}