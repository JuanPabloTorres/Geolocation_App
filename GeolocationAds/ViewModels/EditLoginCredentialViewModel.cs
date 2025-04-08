using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.Services;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class EditLoginCredentialViewModel : BaseViewModel<ToolsLibrary.Models.Login, ILoginService>
    {
        public EditLoginCredentialViewModel(ToolsLibrary.Models.Login model, ILoginService service, LogUserPerfilTool logUserPerfil) : base(model, service, logUserPerfil)
        {
            this.Model = LogUserPerfilTool.LogUser.Login;

            UpdateModel();

            RegisterForLoginUpdates();

            RegisterForSignOutMessage();
        }

        public void UpdateModel()
        {
            IsInternalUser = LogUserPerfilTool.LogUser.Login.IsInternalUser();
        }

        private void RegisterForLoginUpdates()
        {
            WeakReferenceMessenger.Default.Register<UpdateMessage<ToolsLibrary.Models.Login>>(this, (r, m) =>
            {
                if (m.Value is ToolsLibrary.Models.Login login)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Model = login;

                        IsInternalUser = login.IsInternalUser();
                    });
                }
            });
        }

        protected override async Task OnSignOutMessageReceivedAsync()
        {
            await Shell.Current.Navigation.PopAsync();
        }
    }
}