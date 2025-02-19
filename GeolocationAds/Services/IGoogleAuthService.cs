using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeolocationAds.Services
{
    public interface IGoogleAuthService
    {
        Task<string> AuthenticateAndRetrieveTokenAsync();
        Task<JObject> GetGoogleUserInfoAsync(string accessToken);
    }
}
