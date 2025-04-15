using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ToolsLibrary.Models;

namespace GeolocationAds.ViewModels
{
    public partial class FilterPopUpViewModel2 : ObservableObject
    {
        [ObservableProperty]
        private AppSetting selectedAdType;

        [ObservableProperty]
        private string selectedDistance;

        [ObservableProperty]
        private bool isLoading;

        public ObservableCollection<string> DistanceSettings { get; set; }

        public ObservableCollection<AppSetting> AdTypesSettings { get; set; }

        public FilterPopUpViewModel2(IEnumerable<AppSetting> appSettings, IEnumerable<string> distanceSettings)
        {
            this.DistanceSettings = new ObservableCollection<string>(distanceSettings);

            this.AdTypesSettings = new ObservableCollection<AppSetting>(appSettings);

            SelectedAdType = AdTypesSettings.FirstOrDefault();

            SelectedDistance = DistanceSettings.FirstOrDefault();
        }

        public FilterPopUpViewModel2(IEnumerable<AppSetting> appSettings)
        {
            this.AdTypesSettings = new ObservableCollection<AppSetting>(appSettings);

            SelectedAdType = AdTypesSettings.FirstOrDefault();
        }

        public Action OnFilterItem { get; set; }

        public virtual void FilterItemInvoke()
        {
            OnFilterItem?.Invoke();
        }


        [RelayCommand]
        public void Filter()
        {


            FilterItemInvoke();
        }
    }
}