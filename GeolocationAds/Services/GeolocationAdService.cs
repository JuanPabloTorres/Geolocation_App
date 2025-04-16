using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using ToolsLibrary.Extensions;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public class GeolocationAdService : BaseService<GeolocationAd>, IGeolocationAdService
    {
        public static string _apiSuffix = nameof(GeolocationAd);

        public GeolocationAdService(HttpClient htppClient, IConfiguration configuration) : base(htppClient, configuration)
        {
        }

        public async Task<ResponseTool<IEnumerable<GeolocationAd>>> FindAdsNearby(CurrentLocation currentLocation, string distance, int settinTypeId)
        {
            return await HandleRequest<IEnumerable<GeolocationAd>>(async () =>
            {
                var json = JsonConvert.SerializeObject(currentLocation);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{this.BaseApiUri}/{nameof(FindAdsNearby)}/{distance}/{settinTypeId}";

                return await _httpClient.PostAsync(url, content);
            });
        }

        public async Task<ResponseTool<IEnumerable<Advertisement>>> FindAdNear2(CurrentLocation currentLocation, string distance, int settinTypeId, int? pageIndex = 1)
        {
            return await HandleRequest<IEnumerable<Advertisement>>(async () =>
            {
                var json = JsonConvert.SerializeObject(currentLocation);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{this.BaseApiUri}/{nameof(FindAdNear2)}/{distance}/{settinTypeId}/{pageIndex}";

                return await _httpClient.PostAsync(url, content);
            });
        }

        public async Task<ResponseTool<IEnumerable<Advertisement>>> FindAdNear2Streamed(CurrentLocation currentLocation, string distance, int settinTypeId, int? pageIndex = 1)
        {
            return await HandleRequest<IEnumerable<Advertisement>>(
                async () =>
                {
                    var json = JsonConvert.SerializeObject(currentLocation);

                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var url = $"{this.BaseApiUri}/{nameof(FindAdNear2)}/{distance}/{settinTypeId}/{pageIndex}";

                    return await _httpClient.PostAsync(url, content);
                },
                async response =>
                {
                    //var result = new List<Advertisement>();

                    //await using var stream = await response.Content.ReadAsStreamAsync();

                    //using var streamReader = new StreamReader(stream);

                    //using var jsonReader = new JsonTextReader(streamReader);

                    //var serializer = new JsonSerializer();

                    //// Leer el objeto del tipo ResponseTool<List<Advertisement>>
                    //var wrapper = serializer.Deserialize<ResponseTool<List<Advertisement>>>(jsonReader);

                    //if (wrapper == null || !wrapper.IsSuccess || wrapper.Data == null)
                    //{
                    //    return ResponseFactory<IEnumerable<Advertisement>>.BuildFail(
                    //        wrapper?.Message ?? "No data received from server.",
                    //        null,
                    //        ToolsLibrary.Tools.Type.Fail);
                    //}

                    //foreach (var ad in wrapper.Data)
                    //{
                    //    result.Add(ad);

                    //    // 🔄 Puedes emitir el item aquí a la UI con una callback si lo deseas await NotifyItemAsync(ad);
                    //    await Task.Yield(); // mantiene la operación como asíncrona
                    //}

                    //return ResponseFactory<IEnumerable<Advertisement>>.BuildSuccess(
                    //    "Advertisements streamed successfully.",
                    //    result,
                    //    ToolsLibrary.Tools.Type.DataFound);

                    await using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                    using var streamReader = new StreamReader(stream);

                    using var jsonReader = new JsonTextReader(streamReader);

                    var serializer = JsonSerializer.CreateDefault(new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore,
                        DefaultValueHandling = DefaultValueHandling.Ignore
                    });

                    // 🔹 Usa streaming de JSON con buffering eficiente
                    var wrapper = serializer.Deserialize<ResponseTool<List<Advertisement>>>(jsonReader);

                    if (wrapper?.IsSuccess != true || wrapper.Data is null)
                    {
                        return ResponseFactory<IEnumerable<Advertisement>>.BuildFail(
                            wrapper?.Message ?? "No data received from server.",
                            null,
                            ToolsLibrary.Tools.Type.Fail);
                    }

                    // ⚡️ Evita crear una lista nueva innecesaria (usa directamente)
                    return ResponseFactory<IEnumerable<Advertisement>>.BuildSuccess(
                        "Advertisements streamed successfully.",
                        wrapper.Data,
                        ToolsLibrary.Tools.Type.DataFound);

                });
        }

        public async Task<ResponseTool<IEnumerable<GeolocationAd>>> FindNearByStreaming(CurrentLocation currentLocation, string distance, int settinTypeId)
        {
            return await HandleRequest<IEnumerable<GeolocationAd>>(async () =>
            {
                var json = JsonConvert.SerializeObject(currentLocation);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{this.BaseApiUri}/{nameof(FindNearByStreaming)}/{distance}/{settinTypeId}";

                return await _httpClient.PostAsync(url, content);
            }, async response =>
            {
                await using var stream = await response.Content.ReadAsStreamAsync();

                using var reader = new StreamReader(stream);

                using var jsonReader = new JsonTextReader(reader);

                var serializer = new JsonSerializer();

                var wrapper = serializer.Deserialize<ResponseTool<List<GeolocationAd>>>(jsonReader);

                if (wrapper.IsObjectNull())
                {
                    return ResponseFactory<IEnumerable<GeolocationAd>>.BuildFail("Empty or invalid response.", null);
                }

                if (!wrapper.IsSuccess || wrapper.Data.IsObjectNull() || !wrapper.Data.Any())
                {
                    return ResponseFactory<IEnumerable<GeolocationAd>>.BuildFail(
                        "No nearby content found.",
                        Enumerable.Empty<GeolocationAd>(),
                        ToolsLibrary.Tools.Type.NotFound);
                }

                return ResponseFactory<IEnumerable<GeolocationAd>>.BuildSuccess(
                    "Entities streamed and parsed successfully.",
                    wrapper.Data,
                    ToolsLibrary.Tools.Type.DataFound);
            });
        }
    }
}