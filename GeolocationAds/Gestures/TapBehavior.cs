using System.Windows.Input;

namespace GeolocationAds.Gestures
{
    [ContentProperty(nameof(Command))]
    public class TapBehavior : Behavior<View> // Use View instead of VisualElement
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(TapBehavior), null);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        protected override void OnAttachedTo(View bindable) // Use View instead of VisualElement
        {
            base.OnAttachedTo(bindable);
            bindable.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => Command?.Execute(bindable)),
            });
        }

        protected override void OnDetachingFrom(View bindable) // Use View instead of VisualElement
        {
            base.OnDetachingFrom(bindable);
            var tapGesture = bindable.GestureRecognizers.OfType<TapGestureRecognizer>().FirstOrDefault();
            if (tapGesture != null)
                bindable.GestureRecognizers.Remove(tapGesture);
        }
    }
}
