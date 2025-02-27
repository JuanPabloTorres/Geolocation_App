using GeolocationAds.Services;

using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class EditLoginCredentialViewModel : BaseViewModel3<ToolsLibrary.Models.Login, ILoginService>
    {
        public EditLoginCredentialViewModel(ToolsLibrary.Models.Login model, ILoginService service, LogUserPerfilTool logUserPerfil) : base(model, service, logUserPerfil)
        {
            this.Model = logUserPerfil.LogUser.Login;
        }
    }
}