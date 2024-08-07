using GeolocationAds.TemplateViewModel;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;

namespace GeolocationAds.DataTemplates
{
    internal class ContentViewHomeListItemSelector : DataTemplateSelector
    {
        public DataTemplate VideoDataTemplate { get; set; }

        public DataTemplate ImageDataTemplate { get; set; }

        public DataTemplate UrlDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is NearByTemplateViewModel2 contentType)
            {
                if (contentType.Advertisement.Contents.Count == 0 || contentType.Advertisement.Contents.IsObjectNull())
                {
                    return ImageDataTemplate;
                }

                if (contentType.Advertisement.Contents.FirstOrDefault().Type == ContentVisualType.Video)
                {
                    return VideoDataTemplate;
                }
                else if (contentType.Advertisement.Contents.FirstOrDefault().Type == ContentVisualType.Image)
                {
                    return ImageDataTemplate;
                }
                if (contentType.Advertisement.Contents.FirstOrDefault().Type == ContentVisualType.URL)
                {
                    return UrlDataTemplate;
                }
            }

            // Return a default template or null if needed
            return ImageDataTemplate;
        }
    }
}