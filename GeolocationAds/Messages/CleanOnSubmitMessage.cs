using CommunityToolkit.Mvvm.Messaging.Messages;

namespace GeolocationAds.Messages
{
    public class CleanOnSubmitMessage<T> : ValueChangedMessage<T>
    {
        public CleanOnSubmitMessage(T value) : base(value)
        {
        }
    }
}
