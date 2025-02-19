using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Enums;

namespace GeolocationAds.Services
{
    public interface ISecureStoreService
    {
        Task SaveAsync(Providers provider, string username = null, string password = null, string googleClientId = null, bool isRemember = false);
        Task<string> GetAsync(string key);
        Task ClearAll();
    }
}
