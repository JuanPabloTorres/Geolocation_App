using GeolocationAds.TemplateViewModel;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;

namespace GeolocationAds.DataTemplates
{
    public class ContentViewTemplateSelector : DataTemplateSelector
    {
        public DataTemplate VideoDataTemplate { get; set; }

        public DataTemplate ImageDataTemplate { get; set; }

        public DataTemplate DefaultDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is ContentViewTemplateViewModel contentType)
            {
                if (contentType.CurrentAdvertisement.Contents.Count == 0 || contentType.CurrentAdvertisement.Contents.IsObjectNull())
                {
                    return DefaultDataTemplate;
                }


                if (contentType.CurrentAdvertisement.Contents.FirstOrDefault().Type == ContentVisualType.Video)
                {
                    return VideoDataTemplate;
                }
                else if (contentType.CurrentAdvertisement.Contents.FirstOrDefault().Type == ContentVisualType.Image)
                {
                    return ImageDataTemplate;
                }
            }

            // Return a default template or null if needed
            return DefaultDataTemplate;
        }
    }
}
