using CommunityToolkit.Mvvm.Messaging.Messages;
using ToolsLibrary.Models;

namespace GeolocationAds.Messages
{
    public class SignOutMessage : ValueChangedMessage<string>
    {
        public SignOutMessage(string reason = "SessionExpired") : base(reason)
        {
        }


    }
}
