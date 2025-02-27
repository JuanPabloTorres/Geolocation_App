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
        /// Crea un objeto Login con la información del usuario.
        /// </summary>
        /// <param name="email">
        /// Correo electrónico del usuario.
        /// </param>
        /// <param name="googleId">
        /// ID de Google (opcional).
        /// </param>
        /// <param name="facebookId">
        /// ID de Facebook (opcional).
        /// </param>
        /// <param name="provider">
        /// Proveedor de autenticación (por defecto, App).
        /// </param>
        /// <returns>
        /// Instancia de Login.
        /// </returns>
        ToolsLibrary.Models.Login CreateLogin(string email, string? googleId = null, string? facebookId = null, Providers provider = Providers.App);

        /// <summary>
        /// Crea una credencial de usuario basada en un proveedor (Google/Facebook).
        /// </summary>
        /// <param name="providerId">
        /// ID del proveedor (Google/Facebook).
        /// </param>
        /// <param name="provider">
        /// Proveedor de autenticación.
        /// </param>
        /// <returns>
        /// Instancia de Login con credenciales básicas.
        /// </returns>
        ToolsLibrary.Models.Login CreateCredential(string providerId, Providers provider);
    }
}