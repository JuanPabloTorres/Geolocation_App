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
using GeolocationAds.AppTools;

namespace GeolocationAds.ViewModels
{
    public partial class AppShellViewModel2 : RootBaseViewModel
    {
        [ObservableProperty]
        private string userName;

        public AppShellViewModel2()
        {
            WeakReferenceMessenger.Default.Register<UpdateMessage<User>>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var user = m.Value;

                    UserName = user.FullName;

                    Avatar = !string.IsNullOrWhiteSpace(UserName) ? UserName.Trim()[0].ToString().ToUpper() : "?";

                    HasProfileImage = user.ProfileImageBytes?.Length > 0;

                    if (HasProfileImage)
                    {
                        ProfileImage = AppToolCommon.LoadImageFromBytes(user.ProfileImageBytes);
                    }
                    else
                    {
                        ProfileImage = null;
                    }
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