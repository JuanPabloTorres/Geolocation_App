using CommunityToolkit.Mvvm.Messaging.Messages;
using ToolsLibrary.Dto;

namespace GeolocationAds.Messages
{
    public class FilterMessage : ValueChangedMessage<FilterDto>
    {
        public FilterMessage(FilterDto value) : base(value)
        {
        }
    }
}
