using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeolocationAds.AppTools;
using GeolocationAds.Services;
using GeolocationAds.Services.Services_Containers;
using GeolocationAds.TemplateViewModel;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.Maps;
using System.Collections.ObjectModel;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class NearByItemDetailViewModel : BaseViewModel<Advertisement, IAdvertisementService>
    {
        private readonly INearByItemDetailContainer nearByItemDetailContainer;

        [ObservableProperty]
        private string? settingType;

        [ObservableProperty]
        private bool isExpanded;

        public string DisplayDescription => IsExpanded ? this.Model.Description : TruncateDescription(this.Model.Description);

        public NearByItemDetailViewModel(INearByItemDetailContainer nearByItemDetailContainer) : base(nearByItemDetailContainer.Model, nearByItemDetailContainer.AdvertisementService, nearByItemDetailContainer.LogUserPerfilTool)
        {
            this.nearByItemDetailContainer = nearByItemDetailContainer;

            this.ApplyQueryAttributesCompleted = async () => await EditAdvertismentViewModel_ApplyQueryAttributesCompleted();
        }

        private string TruncateDescription(string description)
        {
            return description.Length > ConstantsTools.MaxLengthWithoutExpand ? description.Substring(0, ConstantsTools.MaxLengthWithoutExpand) + "..." : description;
        }

        [RelayCommand]
        private void ToggleExpand()
        {
            IsExpanded = !IsExpanded;

            OnPropertyChanged(nameof(DisplayDescription));
        }

        public ObservableCollection<ContentTypeTemplateViewModel2> ContentTypesTemplate { get; set; } = new ObservableCollection<ContentTypeTemplateViewModel2>();

        private async Task EditAdvertismentViewModel_ApplyQueryAttributesCompleted()
        {
            await this.RunWithLoadingIndicator(async () =>
            {
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
            });
        }

        [RelayCommand]
        public async Task OpenUrl(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                UrlWebViewSource source = new UrlWebViewSource
                {
                    Url = url
                };

                await Launcher.Default.TryOpenAsync(source.Url);
            }
        }

        [RelayCommand]
        public async Task SendEmail(string email)
        {
            await RunWithLoadingIndicator(async () =>
            {
                if (!string.IsNullOrWhiteSpace(email) && Email.Default.IsComposeSupported)
                {
                    var message = new EmailMessage
                    {
                        Subject = "Consulta sobre tu anuncio",
                        Body = "",
                        To = new List<string> { email }
                    };

                    await Email.Default.ComposeAsync(message);
                }
            });
        }

        [RelayCommand]
        public async Task Call(string phone)
        {
            if (!string.IsNullOrWhiteSpace(phone))
                await Launcher.Default.OpenAsync($"tel:{phone}");
        }

        [RelayCommand]
        public async Task OpenMap(AdvertisementMetadata metadata)
        {
            if (metadata.Latitude.HasValue && metadata.Longitude.HasValue)
            {
                var location = new Location(metadata.Latitude.Value, metadata.Longitude.Value);

                var options = new MapLaunchOptions { Name = "Ad Location" };

                await Microsoft.Maui.ApplicationModel.Map.Default.OpenAsync(location, options);
            }
        }
    }
}