namespace GeolocationAds.CustomContentView;

public class CustomStepContentView : ContentView
{
    public static new readonly BindableProperty IsVisibleProperty =
         BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(CustomEntryContentView), default(bool), BindingMode.TwoWay);

    public new bool IsVisible
    {
        get => (bool)GetValue(IsVisibleProperty);
        set => SetValue(IsVisibleProperty, value);
    }

    public static readonly BindableProperty StepIndexProperty =
            BindableProperty.Create(nameof(StepIndex), typeof(int), typeof(CustomStepContentView), default(int));

    public int StepIndex
    {
        get => (int)GetValue(StepIndexProperty);
        set => SetValue(StepIndexProperty, value);
    }

    public static readonly BindableProperty ExecuteCommandProperty =
           BindableProperty.Create(nameof(ExecuteCommand), typeof(Command), typeof(CustomStepContentView), default(Command));

    public Command ExecuteCommand
    {
        get => (Command)GetValue(ExecuteCommandProperty);
        set => SetValue(ExecuteCommandProperty, value);
    }

    public static readonly BindableProperty BackCommandProperty =
         BindableProperty.Create(nameof(ExecuteCommand), typeof(Command), typeof(CustomStepContentView), default(Command));

    public Command BackCommand
    {
        get => (Command)GetValue(BackCommandProperty);
        set => SetValue(BackCommandProperty, value);
    }

    public static readonly BindableProperty ForwardCommandProperty =
        BindableProperty.Create(nameof(ExecuteCommand), typeof(Command), typeof(CustomStepContentView), default(Command));

    public Command ForwardCommand
    {
        get => (Command)GetValue(ForwardCommandProperty);
        set => SetValue(ForwardCommandProperty, value);
    }

    public CustomStepContentView()
    {
        Content = new StackLayout
        {
            Children =
                {
                    new VerticalStackLayout
                    {
                        IsVisible = IsVisible, // Use your binding here
                        Children =
                        {
                            new Label { Style = App.Current.Resources["globalLabel"] as Style, Text = "Password Recovery" },
                            new CustomEntryContentView { Placeholder = "Enter your email", Text = "{Binding Email}" },
                            new Button { Command = ExecuteCommand, IsVisible = true, Style = App.Current.Resources["globalButton"] as Style, Text = "Send Recovery Code" }
                        }
                    },
                    new VerticalStackLayout
                    {
                        IsVisible = IsVisible, // Use your binding here
                        Children =
                        {
                            new Label { Style = App.Current.Resources["globalLabel"] as Style, Text = "Confirm Code Recovery" },
                            new CustomEntryContentView { Placeholder = "Confirm Code", Text = "{Binding Email}" },
                            new Button { Command =ExecuteCommand, IsVisible = true, Style = App.Current.Resources["globalButton"] as Style, Text = "Confirm Code" }
                        }
                    },
                    new VerticalStackLayout
                    {
                        IsVisible = IsVisible, // Use your binding here
                        Children =
                        {
                            new Label { Style = App.Current.Resources["globalLabel"] as Style, Text = "New Password" },
                            new CustomEntryContentView { Placeholder = "New Password", Text = "{Binding Email}" },
                            new Button { Command = ExecuteCommand, IsVisible = true, Style = App.Current.Resources["globalButton"] as Style, Text = "Complete" }
                        }
                    },
                }
        };
    }
}