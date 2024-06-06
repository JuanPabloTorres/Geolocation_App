using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ToolsLibrary.Models;

namespace GeolocationAds.ViewModels
{
    public partial class FilterPopUpViewModel : BaseViewModel
    {
        //public ICommand FilterCommand { get; set; }

        [ObservableProperty]
        private AppSetting selectedAdType;

        //public AppSetting SelectedAdType
        //{
        //    get => _selectedAdType;
        //    set
        //    {
        //        if (_selectedAdType != value)
        //        {
        //            _selectedAdType = value;

        //            OnPropertyChanged();
        //        }
        //    }
        //}

        [ObservableProperty]
        private string selectedDistance;

        //public string SelectedDistance
        //{
        //    get => _selectedDistance;
        //    set
        //    {
        //        if (_selectedDistance != value)
        //        {
        //            _selectedDistance = value;

        //            OnPropertyChanged();
        //        }
        //    }
        //}

        public ObservableCollection<string> DistanceSettings { get; set; }

        public ObservableCollection<AppSetting> AdTypesSettings { get; set; }

        public FilterPopUpViewModel(IEnumerable<AppSetting> appSettings, IEnumerable<string> distanceSettings)
        {
            this.DistanceSettings = new ObservableCollection<string>(distanceSettings);

            this.AdTypesSettings = new ObservableCollection<AppSetting>(appSettings);

            SelectedAdType = AdTypesSettings.FirstOrDefault();

            SelectedDistance = DistanceSettings.FirstOrDefault();

            //this.FilterCommand = new Command(OnsubmitFilter);
        }

        public FilterPopUpViewModel(IEnumerable<AppSetting> appSettings)
        {
            this.AdTypesSettings = new ObservableCollection<AppSetting>(appSettings);

            SelectedAdType = AdTypesSettings.FirstOrDefault();

            //this.FilterCommand = new Command(OnsubmitFilter);
        }

        public delegate void SubmitFilterEventHandler(object sender, EventArgs e);

        public event SubmitFilterEventHandler OnFilterItem;

        public virtual void FilterItemInvoke(EventArgs e)
        {
            OnFilterItem?.Invoke(this, e);
        }

        [RelayCommand]
        public void Filter()
        {
            FilterItemInvoke(EventArgs.Empty);
        }
    }
}