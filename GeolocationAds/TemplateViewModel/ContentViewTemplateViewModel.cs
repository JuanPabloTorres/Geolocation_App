using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FFImageLoading.Maui;
using GeolocationAds.AppTools;
using GeolocationAds.Pages;
using GeolocationAds.Services;
using GeolocationAds.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Extensions;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.TemplateViewModel
{
    public partial class ContentViewTemplateViewModel : BaseTemplateViewModel
    {
        [ObservableProperty]
        private Advertisement _currentAdvertisement;

        [ObservableProperty]
        private MediaSource mediaSource;

        [ObservableProperty]
        private Image image;

        public ContentViewTemplateViewModel(IAdvertisementService advertisementService, IGeolocationAdService geolocationAdService, Advertisement advertisement) : base(advertisementService, geolocationAdService)
        {
            this.CurrentAdvertisement = advertisement;

            Task.Run(async () =>
            {
                await InitializeAsync();
            });
        }

        public async Task InitializeAsync()
        {
            try
            {
                var contents = this.CurrentAdvertisement.Contents.FirstOrDefault();

                if (contents == null)
                {
                    contents = await AppToolCommon.GetDefaultContentType(this.CurrentAdvertisement.CreateBy);
                }

                switch (contents.Type)
                {
                    case ContentVisualType.Image:

                        if (this.Image.IsObjectNull())
                        {
                            this.Image = new Image();
                        }

                        byte[] imageBytes = contents.Content;

                        this.Image.Source = AppToolCommon.LoadImageFromBytes(imageBytes);

                        break;

                    case ContentVisualType.Video:

                        var streamingResponse = await this.advertisementService.GetStreamingVideoUrl(contents.ID);

                        if (!streamingResponse.IsSuccess)
                        {
                            await CommonsTool.DisplayAlert("Error", streamingResponse.Message);

                            return;
                        }

                        this.MediaSource = streamingResponse.Data;

                        break;
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

        [RelayCommand]
        public async Task OpenActionPopUp()
        {
            try
            {
                this.IsLoading = true;

                string action = await Shell.Current.DisplayActionSheet("Actions: ", "Cancel", null, "Detail", "Set Location", "Manage Location");

                switch (action)
                {
                    case "Detail":

                        await Navigate(this.CurrentAdvertisement.ID);

                        break;

                    case "Set Location":

                        await SetLocationYesOrNoAlert(this.CurrentAdvertisement);

                        break;

                    case "Manage Location":

                        var navigationParameter = new Dictionary<string, object> { { "ID", this.CurrentAdvertisement.ID } };

                        await NavigateAsync(nameof(ManageLocation), navigationParameter);

                        break;

                    default:

                        break;
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

        private async Task CreateAdToLocation(Advertisement ad)
        {
            try
            {
                this.IsLoading = true;

                var locationReponse = await GeolocationTool.GetLocation();

                if (!locationReponse.IsSuccess)
                {
                    await CommonsTool.DisplayAlert("Error", locationReponse.Message);

                    return;
                }

                GeolocationAd newAd = GeolocationAdFactory.CreateGeolocationAd(ad, locationReponse.Data);

                var _apiResponse = await this.geolocationAdService.Add(newAd);

                if (_apiResponse.IsSuccess)
                {
                    this.CurrentAdvertisement = ad;

                    await CommonsTool.DisplayAlert("Notification", _apiResponse.Message);
                }
                else
                {
                    await CommonsTool.DisplayAlert("Error", _apiResponse.Message);
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

        private async Task RemoveContent(Advertisement ad)
        {
            try
            {
                this.IsLoading = true;

                var _apiResponse = await this.advertisementService.Remove(ad.ID);

                if (_apiResponse.IsSuccess)
                {
                    RemoveCurrentItem();

                    await CommonsTool.DisplayAlert("Notification", _apiResponse.Message);
                }
                else
                {
                    await CommonsTool.DisplayAlert("Error", _apiResponse.Message);
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

        private async Task SetLocationYesOrNoAlert(Advertisement selectAd)
        {
            if (!selectAd.IsObjectNull())
            {
                var response = await Shell.Current.DisplayAlert("Notification", $"Do you want to set the current location to {selectAd.Title} ?", "Yes", "No");

                if (response)
                {
                    await this.CreateAdToLocation(selectAd);
                }
            }
        }

        private async Task Navigate(int id)
        {
            var navigationParameter = new Dictionary<string, object> { { "ID", id } };

            await Shell.Current.GoToAsync(nameof(EditAdvertisment), navigationParameter);
        }

        [RelayCommand]
        public async Task RemoveContentYesOrNoAlert(Advertisement selectAd)
        {
            if (!selectAd.IsObjectNull())
            {
                var response = await Shell.Current.DisplayAlert("Notification", $"Are you sure you want to remove this item?", "Yes", "No");

                if (response)
                {
                    await this.RemoveContent(selectAd);
                }
            }
        }

        public override void RemoveCurrentItem()
        {
            OnDeleteItem(EventArgs.Empty);
        }
    }
}