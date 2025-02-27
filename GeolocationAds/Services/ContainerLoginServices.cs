using GeolocationAds.Factories;
using GeolocationAds.Services.Services_Containers;
using GeolocationAds.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public class ContainerLoginServices : BaseContainer, IContainerLoginServices
    {
        public ContainerLoginServices(
            ToolsLibrary.Models.Login model,
            ILoginService loginService,
            IForgotPasswordService forgotPasswordService,
            IUserService userService,
            RecoveryPasswordViewModel recoveryPasswordViewModel,
            LogUserPerfilTool logUserPerfil,
            AppShellViewModel2 appShellViewModel2,
            IConfiguration configuration,
            IGoogleAuthService googleAuthService,
            ISecureStoreService secureStoreService, ILoginFactory loginFactory,IFirebaseAuthService firebaseAuthService,
        IUserFactory userFactory, FacebookAuthWebViewViewModel facebookAuthWebViewViewModel)
            : base(logUserPerfil,appShellViewModel2)
        {
            LoginService = loginService;

            ForgotPasswordService = forgotPasswordService;

            UserService = userService;

            RecoveryPasswordViewModel = recoveryPasswordViewModel;

            LoginModel = model;

            Configuration = configuration;

            GoogleAuthService = googleAuthService;

            SecureStoreService = secureStoreService;

            this.LoginFactory = loginFactory;

            this.UserFactory = userFactory;

            this.FirebaseAuthService = firebaseAuthService;

            FacebookAuthWebViewViewModel = facebookAuthWebViewViewModel;
        }

        public IConfiguration Configuration { get; }
        public IForgotPasswordService ForgotPasswordService { get; }
        public IGoogleAuthService GoogleAuthService { get; set; }
        public ISecureStoreService SecureStoreService { get; set; }
        public ILoginFactory LoginFactory { get; }
        public IUserFactory UserFactory { get; }
        public ToolsLibrary.Models.Login LoginModel { get; }
        public ILoginService LoginService { get; }
        public RecoveryPasswordViewModel RecoveryPasswordViewModel { get; set; }
        public IUserService UserService { get; }
        public IFirebaseAuthService FirebaseAuthService { get; set ; }
        public FacebookAuthWebViewViewModel FacebookAuthWebViewViewModel { get ; set ; }
    }
}