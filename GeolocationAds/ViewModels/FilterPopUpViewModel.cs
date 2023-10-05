using System.Collections.ObjectModel;
using System.Windows.Input;
using ToolsLibrary.Models;

namespace GeolocationAds.ViewModels
{
    public class FilterPopUpViewModel : BaseViewModel
    {
        public ICommand FilterCommand { get; set; }

        private AppSetting _selectedAdType;

        public AppSetting SelectedAdType
        {
            get => _selectedAdType;
            set
            {
                if (_selectedAdType != value)
                {
                    _selectedAdType = value;

                    OnPropertyChanged();
                }
            }
        }

        private string _selectedDistance;

        public string SelectedDistance
        {
            get => _selectedDistance;
            set
            {
                if (_selectedDistance != value)
                {
                    _selectedDistance = value;

                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> DistanceSettings { get; set; }

        public ObservableCollection<AppSetting> AdTypesSettings { get; set; }

        public FilterPopUpViewModel(IEnumerable<AppSetting> appSettings, IEnumerable<string> distanceSettings)
        {
            this.DistanceSettings = new ObservableCollection<string>(distanceSettings);

            this.AdTypesSettings = new ObservableCollection<AppSetting>(appSettings);

            SelectedAdType = AdTypesSettings.FirstOrDefault();

            SelectedDistance = DistanceSettings.FirstOrDefault();

            this.FilterCommand = new Command(OnsubmitFilter);
        }

        public delegate void SubmitFilterEventHandler(object sender, EventArgs e);

        public static event SubmitFilterEventHandler OnFilterItem;

        public virtual void FilterItemInvoke(EventArgs e)
        {
            OnFilterItem?.Invoke(this, e);
        }

        public void OnsubmitFilter()
        {
            FilterItemInvoke(EventArgs.Empty);
        }
    }
}