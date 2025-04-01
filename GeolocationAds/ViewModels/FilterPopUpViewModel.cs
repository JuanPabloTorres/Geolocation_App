using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ToolsLibrary.Models;

namespace GeolocationAds.ViewModels
{
    public partial class FilterPopUpViewModel : RootBaseViewModel
    {
        [ObservableProperty]
        private AppSetting selectedAdType;

        [ObservableProperty]
        private string selectedDistance;

        public ObservableCollection<string> DistanceSettings { get; set; }

        public ObservableCollection<AppSetting> AdTypesSettings { get; set; }

        public FilterPopUpViewModel(IEnumerable<AppSetting> appSettings, IEnumerable<string> distanceSettings)
        {
            this.DistanceSettings = new ObservableCollection<string>(distanceSettings);

            this.AdTypesSettings = new ObservableCollection<AppSetting>(appSettings);

            SelectedAdType = AdTypesSettings.FirstOrDefault();

            SelectedDistance = DistanceSettings.FirstOrDefault();
        }

        public FilterPopUpViewModel(IEnumerable<AppSetting> appSettings)
        {
            this.AdTypesSettings = new ObservableCollection<AppSetting>(appSettings);

            SelectedAdType = AdTypesSettings.FirstOrDefault();
        }

        // Reemplazo del delegado personalizado y el evento por un Action
        public Action<FilterPopUpViewModel>? OnFilterItem { get; set; }

        // Método para invocar la acción
        public virtual void FilterItemInvoke()
        {
            OnFilterItem?.Invoke(this);
        }

        [RelayCommand]
        public void Filter()
        {
            FilterItemInvoke();
        }
    }
}