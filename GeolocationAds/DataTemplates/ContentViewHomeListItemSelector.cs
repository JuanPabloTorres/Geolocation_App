using GeolocationAds.TemplateViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;

namespace GeolocationAds.DataTemplates
{
    class ContentViewHomeListItemSelector:DataTemplateSelector
    {
        public DataTemplate VideoDataTemplate { get; set; }

        public DataTemplate ImageDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is NearByTemplateViewModel2 contentType)
            {
                if (contentType.Advertisement.Contents.Count == 0 || contentType.Advertisement.Contents.IsObjectNull())
                {
                    return null;
                }


                if (contentType.Advertisement.Contents.FirstOrDefault().Type == ContentVisualType.Video)
                {
                    return VideoDataTemplate;
                }
                else if (contentType.Advertisement.Contents.FirstOrDefault().Type == ContentVisualType.Image)
                {
                    return ImageDataTemplate;
                }
            }

            // Return a default template or null if needed
            return null;
        }
    }
}
