using GeolocationAds.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ToolsLibrary.Extensions;

namespace ToolsLibrary.TemplateViewModel
{
    public class TemplateBaseViewModel : INotifyPropertyChanged
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

        public ICommand RemoveCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public delegate void RemoveItemEventHandler(object sender, EventArgs e);

        public static event RemoveItemEventHandler ItemDeleted;

        public virtual void OnDeleteItem(EventArgs e)
        {
            ItemDeleted?.Invoke(this, e);
        }

        public TemplateBaseViewModel(IAdvertisementService advertisementService, IGeolocationAdService geolocationAdService)
        {
            this.geolocationAdService = geolocationAdService;

            this.advertisementService = advertisementService;
        }

        public TemplateBaseViewModel(IAdvertisementService advertisementService)
        {
            this.advertisementService = advertisementService;
        }

        public TemplateBaseViewModel()
        {
        }

        public virtual void RemoveCurrentItem()
        {
        }

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
    }
}