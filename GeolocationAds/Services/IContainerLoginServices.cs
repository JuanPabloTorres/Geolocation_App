using GeolocationAds.Factories;
using GeolocationAds.Services.Services_Containers;
using GeolocationAds.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeolocationAds.Services
{
    public interface IContainerLoginServices : IBaseContainer
    {
        IConfiguration Configuration { get; }

        IForgotPasswordService ForgotPasswordService { get; }

        IGoogleAuthService GoogleAuthService { get; set; }

        ISecureStoreService SecureStoreService { get; set; }

        ToolsLibrary.Models.Login LoginModel { get; }

        ILoginService LoginService { get; }

        RecoveryPasswordViewModel RecoveryPasswordViewModel { get; set; }

        IUserService UserService { get; }

        ILoginFactory LoginFactory { get; }

        IUserFactory UserFactory { get; }



        FacebookAuthWebViewViewModel FacebookAuthWebViewViewModel { get; set; }

        GoogleAuthWebViewViewModel GoogleAuthWebViewViewModel { get; set; }
    }
}