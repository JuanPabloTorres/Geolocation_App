//using Microsoft.Maui.ApplicationModel.Communication;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ToolsLibrary.Enums;
//using ToolsLibrary.Models;

//namespace GeolocationAds.Factories
//{
//    public class LoginFactory : ILoginFactory
//    {
//        public ToolsLibrary.Models.Login CreateLogin(string email, string? googleId = null, string? facebookId = null,
//                                              ToolsLibrary.Enums.Providers provider = ToolsLibrary.Enums.Providers.App)
//        {
//            if (string.IsNullOrWhiteSpace(email))
//                throw new ArgumentException("Email cannot be null or empty", nameof(email));

// return new ToolsLibrary.Models.Login { CreateBy = 0, CreateDate = DateTime.UtcNow, // Se usa UTC
// para evitar problemas de zonas horarias GoogleId = googleId, FacebookId = facebookId, Password =
// provider switch { ToolsLibrary.Enums.Providers.Google => "google",
// ToolsLibrary.Enums.Providers.Facebook => "facebook", _ => "default_password" }, Username = email,
// Provider = provider }; }

// public ToolsLibrary.Models.Login CreateFacebookLogin(string email, string facebookId ,
// ToolsLibrary.Enums.Providers provider = ToolsLibrary.Enums.Providers.Facebook) { return new
// ToolsLibrary.Models.Login { CreateBy = 0, CreateDate = DateTime.Now, GoogleId = null,
// FacebookId=facebookId, Password = provider == ToolsLibrary.Enums.Providers.Facebook ? "facebook"
// : "default_password", Username = email, Provider = provider }; }

// public ToolsLibrary.Models.Login CreateGoogleLogin(string email, string googleId,
// ToolsLibrary.Enums.Providers provider = ToolsLibrary.Enums.Providers.Google) { return new
// ToolsLibrary.Models.Login { CreateBy = 0, CreateDate = DateTime.Now, GoogleId = googleId,
// FacebookId = null, Password = provider == ToolsLibrary.Enums.Providers.Google ? "facebook" :
// "default_password", Username = email, Provider = provider }; }

// public ToolsLibrary.Models.Login CreateFacebookCredential(string facebookId) { return new
// ToolsLibrary.Models.Login { GoogleId = null, FacebookId = facebookId, Username = "facebook",
// Password = "default_password", Provider = Providers.Facebook, CreateBy = 0, CreateDate =
// DateTime.Now }; }

//        public ToolsLibrary.Models.Login CreateGoogleCredential(string googleId)
//        {
//            return new ToolsLibrary.Models.Login
//            {
//                GoogleId = googleId,
//                FacebookId=null,
//                Username = "googleuser",
//                Password = "google",
//                Provider = Providers.Google,
//                CreateBy = 0,
//                CreateDate = DateTime.Now
//            };
//        }
//    }
//}

using ToolsLibrary.Enums;
using ToolsLibrary.Models;

namespace GeolocationAds.Factories
{
    public class LoginFactory : ILoginFactory
    {
        public ToolsLibrary.Models.Login CreateLogin(string email, string? googleId = null, string? facebookId = null,
                                 Providers provider = Providers.App)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty", nameof(email));

            return CreateLoginInternal(email, googleId, facebookId, provider);
        }

        public ToolsLibrary.Models.Login CreateCredential(string providerId, Providers provider)
        {
            if (string.IsNullOrWhiteSpace(providerId))
                throw new ArgumentException("Provider ID cannot be null or empty", nameof(providerId));

            return provider switch
            {
                Providers.Google => CreateLoginInternal("googleuser", providerId, null, Providers.Google),
                Providers.Facebook => CreateLoginInternal("facebook", null, providerId, Providers.Facebook),
                _ => throw new NotSupportedException($"Provider {provider} is not supported.")
            };
        }

        private static ToolsLibrary.Models.Login CreateLoginInternal(string email, string? googleId, string? facebookId, Providers provider)
        {
            return new ToolsLibrary.Models.Login
            {
                CreateBy = 0,
                CreateDate = DateTime.UtcNow,
                GoogleId = googleId,
                FacebookId = facebookId,
                Password = provider switch
                {
                    Providers.Google => "google",
                    Providers.Facebook => "facebook",
                    _ => "default_password"
                },
                Username = email,
                Provider = provider
            };
        }
    }
}