using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using System.Windows.Input;
using ToolsLibrary.Models;

namespace GeolocationAds.ViewModels
{
    public partial class AppShellViewModel : BaseViewModel
    {
        public ICommand SignOutCommand { get; set; }

        public string _userName;

        public string UserName
        {
            get => _userName;

            set
            {
                if (_userName != value)
                {
                    _userName = value;

                    OnPropertyChanged();
                }
            }
        }

        public User user { get; set; }

        public AppShellViewModel()
        {
            SignOutCommand = new Command(SignOut);

            WeakReferenceMessenger.Default.Register<LogInMessage<string>>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    this.UserName = m.Value;
                });
            });
        }

        private void SignOut()
        {
            WeakReferenceMessenger.Default.Send(new LogOffMessage(null));
        }
    }
}