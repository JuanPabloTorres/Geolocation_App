using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.TemplateViewModel
{
    public partial class ContentTypeTemplateViewModel2 : TemplateBaseViewModel2
    {
        [ObservableProperty]
        private ContentType contentType;

        [ObservableProperty]
        private MediaSource mediaSource;

        [ObservableProperty]
        private ImageSource image;

        public delegate void ApplyQueryAttributesEventHandler(object sender, EventArgs e);

        public event ApplyQueryAttributesEventHandler ContentTypeDeleted;

        protected virtual void OnDeleteType(EventArgs e)
        {
            ContentTypeDeleted?.Invoke(this, e);
        }

        public ContentTypeTemplateViewModel2()
        {
        }

        public ContentTypeTemplateViewModel2(ContentType contentType, MediaSource mediaSource)
        {
            this.ContentType = contentType;

            this.MediaSource = mediaSource;
        }

        public ContentTypeTemplateViewModel2(ContentType contentType, ImageSource imageSource)
        {
            this.ContentType = contentType;

            this.Image = imageSource;
        }

        [RelayCommand]
        public override async Task RemoveCurrentItem()
        {
            try
            {
                this.IsLoading = true;

                OnDeleteType(EventArgs.Empty);
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
            finally
            {
                this.IsLoading = false;
            }
        }
    }
}