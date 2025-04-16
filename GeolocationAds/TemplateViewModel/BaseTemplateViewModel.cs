using CommunityToolkit.Mvvm.ComponentModel;
using GeolocationAds.Services;
using GeolocationAds.ViewModels;
using ToolsLibrary.Extensions;
using ToolsLibrary.Tools;

namespace GeolocationAds.TemplateViewModel
{
    public partial class BaseTemplateViewModel : RootBaseViewModel
    {
        protected IAdvertisementService advertisementService { get; set; }

        protected IGeolocationAdService geolocationAdService { get; set; }

        public BaseTemplateViewModel(IAdvertisementService advertisementService, IGeolocationAdService geolocationAdService)
        {
            this.geolocationAdService = geolocationAdService;

            this.advertisementService = advertisementService;
        }

        public BaseTemplateViewModel(IAdvertisementService advertisementService)
        {
            this.advertisementService = advertisementService;
        }

        public BaseTemplateViewModel(IGeolocationAdService geolocationAdService)
        {
            this.geolocationAdService = geolocationAdService;
        }

        public BaseTemplateViewModel()
        {
        }

        public  Action<ContentViewTemplateViewModel> ItemDeleted { get; set; }

        public async Task NavigateAsync(string pageName, Dictionary<string, object> queryParameters = null)
        {
            await RunWithLoadingIndicator(async () =>
            {
                if (string.IsNullOrWhiteSpace(pageName))
                {
                    throw new ArgumentException("Page name cannot be null or empty", nameof(pageName));
                }

                var queryString = queryParameters != null && queryParameters.Any()
                    ? "?" + string.Join("&", queryParameters.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value?.ToString() ?? string.Empty)}"))
                    : string.Empty;

                await Shell.Current.GoToAsync($"{pageName}{queryString}");
            });
        }


        public virtual void OnDeleteItem(ContentViewTemplateViewModel sender)
        {
            ItemDeleted?.Invoke(sender);
        }

        public virtual void RemoveCurrentItem()
        {
        }

        public virtual async Task OpenMetaDataPopUp()
        {
            try
            {
                await Shell.Current.DisplayAlert("Notification", "Pop Up Must Override.", "OK");
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }
    }
}