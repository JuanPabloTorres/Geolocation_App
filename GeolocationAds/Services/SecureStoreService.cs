using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Enums;

namespace GeolocationAds.Services
{
    public class SecureStoreService : ISecureStoreService
    {
        private const string ProviderKey = "provider";

        private const string UsernameKey = "username";

        private const string PasswordKey = "password";

        public const string GoogleClientIdKey = "googleClientId";
        
        public const string FacebookClientIdKey = "facebookClientId";

        public const string IsRememberKey = "isRemember";

        /// <summary>
        /// Guarda valores en SecureStorage.
        /// </summary>
        public async Task SaveAsync(Providers provider, string username = null, string password = null, string googleClientId = null,string facebookClientId=null, bool isRemember = false)
        {
            try
            {
                await SecureStorage.SetAsync(IsRememberKey, isRemember.ToString());

                await SecureStorage.SetAsync(ProviderKey, provider.ToString());

                switch (provider)
                {
                    case Providers.App:
                        if (!string.IsNullOrWhiteSpace(username))
                        {
                            await SecureStorage.SetAsync(UsernameKey, username);
                        }

                        if (!string.IsNullOrWhiteSpace(password))
                        {
                            await SecureStorage.SetAsync(PasswordKey, password);
                        }
                        break;

                    case Providers.Google:
                        if (!string.IsNullOrWhiteSpace(googleClientId))
                        {
                            await SecureStorage.SetAsync(GoogleClientIdKey, googleClientId);
                        }
                        break;
                    case Providers.Facebook:
                        if (!string.IsNullOrWhiteSpace(facebookClientId))
                        {
                            await SecureStorage.SetAsync(FacebookClientIdKey, facebookClientId);
                        }
                        break;

                    default:
                        throw new ArgumentException("Unsupported provider type.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save credentials: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene un valor almacenado en SecureStorage.
        /// </summary>
        public async Task<string> GetAsync(string key)
        {
            try
            {
                return await SecureStorage.GetAsync(key);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to retrieve {key}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Elimina todos los datos almacenados en SecureStorage.
        /// </summary>
        public async Task ClearAll()
        {
            try
            {

                SecureStorage.RemoveAll();


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to clear credentials: {ex.Message}");
            }
        }
    }
}
