using Microsoft.Maui.ApplicationModel.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Enums;
using ToolsLibrary.Models;

namespace GeolocationAds.Factories
{
    public class LoginFactory : ILoginFactory
    {
        public ToolsLibrary.Models.Login CreateLogin(string email, string googleId = null, ToolsLibrary.Enums.Providers provider = ToolsLibrary.Enums.Providers.App)
        {
            return new ToolsLibrary.Models.Login
            {
                CreateBy = 0,
                CreateDate = DateTime.Now,
                GoogleId = googleId,
                Password = provider == ToolsLibrary.Enums.Providers.Google ? "google" : "default_password",
                Username = email,
                Provider = provider
            };
        }

        public ToolsLibrary.Models.Login CreateGoogleCredential(string googleId)
        {
            return new ToolsLibrary.Models.Login
            {
                GoogleId = googleId,
                Username = "googleuser",
                Password = "google",
                Provider = Providers.Google,
                CreateBy = 0,
                CreateDate = DateTime.Now
            };
        }
    }
}

