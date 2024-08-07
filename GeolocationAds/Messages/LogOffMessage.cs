using CommunityToolkit.Mvvm.Messaging.Messages;
using ToolsLibrary.Models;

namespace GeolocationAds.Messages
{
    class LogOffMessage : ValueChangedMessage<User>
    {
        public LogOffMessage(User value) : base(value)
        {
        }


    }
}
