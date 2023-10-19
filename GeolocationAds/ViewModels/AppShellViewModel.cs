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

        public string _avatar;

        public string Avatar
        {
            get => _avatar;

            set
            {
                if (_avatar != value)
                {
                    _avatar = value;

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

                    this.Avatar = this.UserName.FirstOrDefault().ToString();
                });
            });

            WeakReferenceMessenger.Default.Register<UpdateMessage<User>>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    this.UserName = m.Value.FullName;

                    this.Avatar = this.UserName.FirstOrDefault().ToString();
                });
            });
        }

        private async void SignOut()
        {
            //WeakReferenceMessenger.Default.Send(new LogOffMessage(null));

            await Shell.Current.GoToAsync(nameof(Login));

            Shell.Current.FlyoutIsPresented = false;
        }
    }
}