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

        public DataTemplate UrlDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is ContentViewTemplateViewModel contentType)
            {
                if (contentType.Advertisement.Contents.Count == 0 || contentType.Advertisement.Contents.IsObjectNull())
                {
                    return DefaultDataTemplate;
                }

                if (contentType.Advertisement.Contents.FirstOrDefault().Type == ContentVisualType.Video)
                {
                    return VideoDataTemplate;
                }
                else if (contentType.Advertisement.Contents.FirstOrDefault().Type == ContentVisualType.Image)
                {
                    return ImageDataTemplate;
                }
                else if (contentType.Advertisement.Contents.FirstOrDefault().Type == ContentVisualType.URL)
                {
                    return UrlDataTemplate;
                }
            }

            // Return a default template or null if needed
            return DefaultDataTemplate;
        }
    }
}