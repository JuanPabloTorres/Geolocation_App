using CommunityToolkit.Mvvm.Messaging.Messages;

namespace GeolocationAds.Messages
{
    public class UpdateMessage<T> : ValueChangedMessage<T>
    {
        public UpdateMessage(T value) : base(value)
        {
        }
    }
}