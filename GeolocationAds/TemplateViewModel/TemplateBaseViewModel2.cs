using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeolocationAds.Services;
using GeolocationAds.ViewModels;
using System.ComponentModel;
using ToolsLibrary.Extensions;
using ToolsLibrary.Tools;

namespace GeolocationAds.TemplateViewModel
{
    public partial class TemplateBaseViewModel2<T> : RootBaseViewModel
    {
        public Action<T> ItemDeleted { get; set; }  // ✅ Se usa Action en vez de event

        protected IAdvertisementService advertisementService { get; set; }
        protected IGeolocationAdService geolocationAdService { get; set; }

        public TemplateBaseViewModel2(IAdvertisementService advertisementService, IGeolocationAdService geolocationAdService)
        {
            this.geolocationAdService = geolocationAdService;

            this.advertisementService = advertisementService;
        }

        public TemplateBaseViewModel2(IAdvertisementService advertisementService)
        {
            this.advertisementService = advertisementService;
        }

        public TemplateBaseViewModel2(IGeolocationAdService geolocationAdService)
        {
            this.advertisementService = advertisementService;
        }

        public TemplateBaseViewModel2()
        {
        }

        [RelayCommand]
        public virtual async Task RemoveCurrentItem()
        {
            try
            {
                await Shell.Current.DisplayAlert("Notification", "Must Implement.", "OK");
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        protected virtual void OnDeleteType(T contentTypeTemplateViewModel2)
        {
            ItemDeleted?.Invoke(contentTypeTemplateViewModel2);  // ✅ Se usa Action en lugar de event
        }

        public async Task NavigateAsync(string pageName, Dictionary<string, object> queryParameters = null)
        {
            var queryString = queryParameters != null && queryParameters.Count > 0
                ? "?" + string.Join("&", queryParameters.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value.ToString())}"))
                : string.Empty;

            await Shell.Current.GoToAsync($"{pageName}{queryString}");
        }
    }
}