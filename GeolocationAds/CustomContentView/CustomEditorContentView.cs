namespace GeolocationAds.CustomContentView
{
    public class CustomEditorContentView : ContentView
    {
        public static readonly BindableProperty TextProperty =
           BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomEntryContentView), default(string), BindingMode.TwoWay);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty PlaceholderProperty =
               BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(CustomEntryContentView), default(string), BindingMode.TwoWay);

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static readonly BindableProperty IsEnabledProperty =
           BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(CustomEntryContentView), default(bool), BindingMode.TwoWay);

        public bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        public static readonly BindableProperty LenghtCountProperty =
         BindableProperty.Create(nameof(LenghtCount), typeof(string), typeof(CustomEntryContentView), "0/180", BindingMode.TwoWay);

        public string LenghtCount
        {
            get => (string)GetValue(LenghtCountProperty);
            set => SetValue(LenghtCountProperty, value);
        }

        public CustomEditorContentView()
        {
            var entry = new Editor
            {
                Style = (Style)Application.Current.Resources["globalEditor"]
            };

            entry.SetBinding(Editor.TextProperty, new Binding(nameof(Text), source: this));

            entry.SetBinding(Editor.PlaceholderProperty, new Binding(nameof(Placeholder), source: this));

            entry.SetBinding(Editor.IsEnabledProperty, new Binding(nameof(IsEnabled), source: this));

            var characterLabel = new Label()
            {
                FontSize = 12,
                FontFamily = "Roboto",
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End,
                Padding = new Thickness(0, 0, 7, 0),
                TextColor = (Color)Application.Current.Resources["AppBlack"]
            };

            characterLabel.SetBinding(Label.TextProperty, new Binding(nameof(LenghtCount), source: this));

            var frame = new Border
            {
                Style = (Style)Application.Current.Resources["CustomBorderStyle"],

                Content = new VerticalStackLayout
                {
                    Children = { entry, characterLabel }
                }
            };

            Content = new VerticalStackLayout
            {
                Margin = 5,
                Children = { frame }
            };

            entry.TextChanged += Entry_TextChanged;
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.LenghtCount = $"{Text.Length}/180";
        }
    }
}