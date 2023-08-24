using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using System.Windows.Input;

namespace GeolocationAds.ViewModels
{
    public class AppShellViewModel
    {
        public ICommand SignOutCommand { get; set; }

        public AppShellViewModel()
        {
            SignOutCommand = new Command(SignOut);
        }

        private void SignOut()
        {
            WeakReferenceMessenger.Default.Send(new LogOffMessage(null));
        }
    }
}