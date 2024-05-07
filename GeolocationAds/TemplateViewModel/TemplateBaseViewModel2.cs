using CommunityToolkit.Mvvm.ComponentModel;
using GeolocationAds.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Tools;

namespace GeolocationAds.TemplateViewModel
{
    public partial class TemplateBaseViewModel2 : ObservableObject
    {
        [ObservableProperty]
        private bool isLoading;

        public delegate void RemoveItemEventHandler(object sender, EventArgs e);

        public static event RemoveItemEventHandler ItemDeleted;

        public event PropertyChangedEventHandler PropertyChanged;

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

        public TemplateBaseViewModel2()
        {
        }

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
    }
}