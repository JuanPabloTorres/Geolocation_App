using CommunityToolkit.Mvvm.ComponentModel;
using GeolocationAds.Services;
using ToolsLibrary.Extensions;
using ToolsLibrary.Tools;

namespace GeolocationAds.TemplateViewModel
{
    public partial class BaseTemplateViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isLoading;

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

        public delegate void RemoveItemEventHandler(object sender, EventArgs e);

        public static event RemoveItemEventHandler ItemDeleted;

        public async Task NavigateAsync(string pageName, Dictionary<string, object> queryParameters = null)
        {
            var queryString = string.Empty;

            if (!queryParameters.IsObjectNull() && queryParameters.Count > 0)
            {
                queryString = string.Join("&", queryParameters.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value.ToString())}"));

                queryString = "?" + queryString;
            }

            await Shell.Current.GoToAsync($"{pageName}{queryString}");
        }

        public virtual void OnDeleteItem(EventArgs e)
        {
            ItemDeleted?.Invoke(this, e);
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