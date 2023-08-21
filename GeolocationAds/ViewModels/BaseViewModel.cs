using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace GeolocationAds.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        protected bool isLoading;

        [ICommand]
        protected virtual void OnSubmitButton()
        {

        }
    }
}