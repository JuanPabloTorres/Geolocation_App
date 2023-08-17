using CommunityToolkit.Mvvm.Messaging.Messages;
using ToolsLibrary.Models;

namespace GeolocationAds.Messages
{
    public class DeleteItemMessage : ValueChangedMessage<Advertisement>
    {
        public DeleteItemMessage(Advertisement value) : base(value)
        {
        }
    }
}