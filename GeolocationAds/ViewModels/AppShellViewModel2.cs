using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeolocationAds.Messages;
using System.Windows.Input;
using ToolsLibrary.Models;

namespace GeolocationAds.ViewModels
{
    public partial class AppShellViewModel2 : ObservableObject
    {
        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string avatar;

        public AppShellViewModel2()
        {
            WeakReferenceMessenger.Default.Register<UpdateMessage<User>>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    this.UserName = m.Value.FullName;

                    this.Avatar = this.UserName.FirstOrDefault().ToString();
                });
            });
        }

        [RelayCommand]
        public async Task SignOut()
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;

            await Shell.Current.GoToAsync(nameof(Login));

            Shell.Current.FlyoutIsPresented = false;
        }
    }
}