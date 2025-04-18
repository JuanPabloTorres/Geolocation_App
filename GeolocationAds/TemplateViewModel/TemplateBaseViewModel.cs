﻿using GeolocationAds.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToolsLibrary.TemplateViewModel
{
    public class TemplateBaseViewModel : INotifyPropertyChanged
    {

        protected IAdvertisementService advertisementService { get; set; }

        protected IGeolocationAdService geolocationAdService { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public TemplateBaseViewModel(IAdvertisementService advertisementService, IGeolocationAdService geolocationAdService)
        {
            this.geolocationAdService = geolocationAdService;

            this.advertisementService = advertisementService;
        }

        public TemplateBaseViewModel()
        {

        }
    }

}
