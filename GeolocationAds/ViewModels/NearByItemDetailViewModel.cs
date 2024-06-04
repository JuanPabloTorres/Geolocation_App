using CommunityToolkit.Mvvm.ComponentModel;
using GeolocationAds.AppTools;
using GeolocationAds.Services;
using GeolocationAds.Services.Services_Containers;
using GeolocationAds.TemplateViewModel;
using System.Collections.ObjectModel;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class NearByItemDetailViewModel : BaseViewModel3<Advertisement, IAdvertisementService>
    {
        private readonly INearByItemDetailContainer nearByItemDetailContainer;

        [ObservableProperty]
        private string? settingType;

        public NearByItemDetailViewModel(INearByItemDetailContainer nearByItemDetailContainer) : base(nearByItemDetailContainer.Model, nearByItemDetailContainer.AdvertisementService, nearByItemDetailContainer.LogUserPerfilTool)
        {
            this.nearByItemDetailContainer = nearByItemDetailContainer;

            this.ApplyQueryAttributesCompleted += EditAdvertismentViewModel_ApplyQueryAttributesCompleted;
        }

        public ObservableCollection<ContentTypeTemplateViewModel2> ContentTypesTemplate { get; set; } = new ObservableCollection<ContentTypeTemplateViewModel2>();

        private async void EditAdvertismentViewModel_ApplyQueryAttributesCompleted(object sender, EventArgs e)
        {
            try
            {
                this.IsLoading = true;

                this.SettingType = this.Model.Settings.First().Setting.Value;

                foreach (var item in this.Model.Contents)
                {
                    if (item.Type == ContentVisualType.Video)
                    {
                        var _template = await AppToolCommon.ProcessContentItem(item, this.service);

                        this.ContentTypesTemplate.Add(_template);
                    }
                    else
                    {
                        var _template = await AppToolCommon.ProcessContentItem(item, null);

                        this.ContentTypesTemplate.Add(_template);
                    }
                }
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