using GeolocationAds.Services;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class EditUserPerfilViewModel : BaseViewModel3<User, IUserService>
    {
        public EditUserPerfilViewModel(User model, IUserService service, LogUserPerfilTool logUserPerfil) : base(model, service, logUserPerfil)
        {
        }
    }
}
