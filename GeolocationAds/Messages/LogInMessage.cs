using CommunityToolkit.Mvvm.Messaging.Messages;

namespace GeolocationAds.Messages
{
    class LogInMessage<T> : ValueChangedMessage<T>
    {
        public LogInMessage(T value) : base(value)
        {
        }
    }
}
