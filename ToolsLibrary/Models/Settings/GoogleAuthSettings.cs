using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsLibrary.Models.Settings
{
    public class GoogleAuthSettings
    {
        public string GoogleClientId { get; set; }
        public string GoogleRedirectUri { get; set; }
        public string GoogleAuthUrl { get; set; }

        /// <summary>
        /// Genera dinámicamente la URL de autenticación con Google.
        /// </summary>
        public string GetAuthUrl()
        {
            return $"{GoogleAuthUrl}?" +
                   $"client_id={GoogleClientId}&" +
                   $"redirect_uri={Uri.EscapeDataString(GoogleRedirectUri)}&" +
                   $"response_type=code&" +
                   $"scope={Uri.EscapeDataString("openid email profile")}";
        }
    }
}
