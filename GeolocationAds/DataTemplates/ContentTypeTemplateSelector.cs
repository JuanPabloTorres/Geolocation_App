using GeolocationAds.AppTools;
using GeolocationAds.TemplateViewModel;
using ToolsLibrary.Models;

namespace GeolocationAds.DataTemplates
{
    public class ContentTypeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate VideoDataTemplate { get; set; }

        public DataTemplate ImageDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is ContentTypeTemplateViewModel2 contentType)
            {
                if (contentType.ContentType.Type == ContentVisualType.Video)
                {
                    return VideoDataTemplate;
                }

                if (contentType.ContentType.Type == ContentVisualType.Image)
                {
                    return ImageDataTemplate;
                }
            }

            // Return a default template or null if needed
            return ImageDataTemplate;
        }
    }
}