using CommunityToolkit.Mvvm.ComponentModel;

namespace GeolocationAds.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isLoading;

        public static string CurrentPageContext { get; set; }
    }
}