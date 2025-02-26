using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Enums;

namespace GeolocationAds.Factories
{
    public interface ILoginFactory
    {
        /// <summary>
        /// Crea una instancia de Login para autenticación tradicional con usuario y contraseña.
        /// </summary>
        ToolsLibrary.Models.Login CreateLogin(string email, string googleId = null, ToolsLibrary.Enums.Providers provider = ToolsLibrary.Enums.Providers.App);

        /// <summary>
        /// Crea una instancia de Login para autenticación con Google.
        /// </summary>
        ToolsLibrary.Models.Login CreateGoogleCredential(string googleId);

        ToolsLibrary.Models.Login CreateFacebookLogin(string email, string facebookId = null, ToolsLibrary.Enums.Providers provider = ToolsLibrary.Enums.Providers.Facebook);
    }
}