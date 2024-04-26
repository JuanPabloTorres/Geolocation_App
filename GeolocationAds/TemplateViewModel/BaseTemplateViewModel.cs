using CommunityToolkit.Mvvm.ComponentModel;
using GeolocationAds.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToolsLibrary.Extensions;

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
    }
}