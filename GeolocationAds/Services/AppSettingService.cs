using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public class AppSettingService : BaseService<AppSetting>, IAppSettingService
    {
        public static string _apiSuffix = nameof(AppSettingService);

        public AppSettingService(HttpClient htppClient, IConfiguration configuration) : base(htppClient, configuration)
        {
        }

        public async Task<ResponseTool<IEnumerable<AppSetting>>> GetAppSettingByName(string name)
        {
            return await HandleRequest<IEnumerable<AppSetting>>(async () =>
            {
                var apiUrl = $"{this.BaseApiUri}/{nameof(GetAppSettingByName)}/{name}";

                return await _httpClient.GetAsync(apiUrl);
            });
        }

        public async Task<ResponseTool<IEnumerable<AppSetting>>> GetAppSettingByNames(IList<string> settinsName)
        {
            return await HandleRequest<IEnumerable<AppSetting>>(async () =>
            {
                var json = JsonConvert.SerializeObject(settinsName);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                return await _httpClient.PostAsync($"{this.BaseApiUri}/{nameof(GetAppSettingByNames)}", content);
            });
        }
    }
}