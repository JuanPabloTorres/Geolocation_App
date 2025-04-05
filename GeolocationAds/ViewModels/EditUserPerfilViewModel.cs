using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using GeolocationAds.AppTools;
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
    public partial class EditUserPerfilViewModel : BaseViewModel<User, IUserService>
    {
        public EditUserPerfilViewModel(User model, IUserService service, LogUserPerfilTool logUserPerfil)
            : base(model, service, logUserPerfil)
        {
            UpdateModel();

            //RegisterForUserUpdates();
        }

        public void UpdateModel()
        {
            Model = LogUserPerfilTool.LogUser;

            Model.Login = LogUserPerfilTool.LogUser.Login;

            HasProfileImage = Model?.ProfileImageBytes?.Length > 0;

            IsInternalUser = Model.Login.IsInternalUser();

            if (HasProfileImage)
            {
                ProfileImage = AppToolCommon.LoadImageFromBytes(Model.ProfileImageBytes);
            }
            else
            {
                Avatar = !string.IsNullOrWhiteSpace(Model.FullName) ? Model.FullName.Trim()[0].ToString().ToUpper() : "?";
            }
        }
    }
}