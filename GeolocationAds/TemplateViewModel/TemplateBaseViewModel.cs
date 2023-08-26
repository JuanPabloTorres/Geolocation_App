using GeolocationAds.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToolsLibrary.TemplateViewModel
{
    public class TemplateBaseViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        private bool isLoading;

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                if (isLoading != value)
                {
                    isLoading = value;

                    OnPropertyChanged();
                }
            }
        }

        protected IAdvertisementService advertisementService { get; set; }

        protected IGeolocationAdService geolocationAdService { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public string ID { get; private set; }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            ID = query["ID"] as string;
        }

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