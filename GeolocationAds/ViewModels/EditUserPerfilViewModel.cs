using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.Services;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class EditUserPerfilViewModel : BaseViewModel4<User, IUserService>
    {
        [ObservableProperty] private User _user;

        public EditUserPerfilViewModel(User model, IUserService service, LogUserPerfilTool logUserPerfil)
            : base(service, logUserPerfil)
        {
            UpdateModel();
        }

        public void UpdateModel()
        {
            User = LogUserPerfilTool.LogUser;
        }
    }
}