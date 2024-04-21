using GeolocationAds.TemplateViewModel;
using GeolocationAds.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;

namespace GeolocationAds.DataTemplates
{
    public class ContentViewTemplateSelector: DataTemplateSelector
    {
        public DataTemplate VideoDataTemplate { get; set; }

        public DataTemplate ImageDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is ContentViewTemplateViewModel contentType)
            {
                if (contentType.CurrentAdvertisement.Contents.Count == 0 || contentType.CurrentAdvertisement.Contents.IsObjectNull())
                {
                    return null;
                }


                if (contentType.CurrentAdvertisement.Contents.FirstOrDefault() .Type== ContentVisualType.Video)
                {
                    return VideoDataTemplate;
                }
                else if (contentType.CurrentAdvertisement.Contents.FirstOrDefault().Type == ContentVisualType.Image)
                {
                    return ImageDataTemplate;
                }
            }

            // Return a default template or null if needed
            return null;
        }
    }
}
