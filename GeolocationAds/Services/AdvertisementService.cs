using ExCSS;
using Newtonsoft.Json;
using System;
using System.Text;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services
{
    public class AdvertisementService : BaseService<Advertisement>, IAdvertisementService
    {
        public AdvertisementService(HttpClient htppClient) : base(htppClient)
        {
        }

        public async Task<ResponseTool<IEnumerable<Advertisement>>> GetAdvertisementsOfUser(int userId, int typeId, int? pageIndex)
        {
            try
            {
                // Build the full API endpoint URL for the "all" endpoint

                // Send an HTTP GET request to the "all" endpoint of your API
                HttpResponseMessage response = await this._httpClient.GetAsync($"{this.BaseApiUri}/{nameof(GetAdvertisementsOfUser)}/{userId}/{typeId}/{pageIndex}");

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Read the response content as a string
                string responseContent = await response.Content.ReadAsStringAsync();

                // Deserialize the response content to your custom ResponseTool<IEnumerable<T>> type
                var result = JsonConvert.DeserializeObject<ResponseTool<IEnumerable<Advertisement>>>(responseContent);

                return result;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the request
                // For simplicity, we'll just return an error ResponseTool with the exception message

                var failResponse = ResponseFactory<IEnumerable<Advertisement>>.BuildFail($"An error occurred: {ex.Message}", null);

                return failResponse;
            }
        }

        public override async Task<ResponseTool<Advertisement>> Add(Advertisement data)
        {
            try
            {
                using var multipartContent = new MultipartFormDataContent();

                // Add the Advertisement metadata as JSON
                var advertisementMetadata = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                multipartContent.Add(new StringContent(advertisementMetadata, Encoding.UTF8, "application/json"), "advertisementMetadata");

                // Add the contents (images, videos, etc.)
                foreach (var content in data.Contents)
                {
                    if (content.Content != null && content.Content.Length > 0)
                    {
                        var contentStream = new MemoryStream(content.Content);

                        var contentName = content.ContentName ?? "file"; // Default file name if not specified

                        var mediaType = GetMediaType(content.Type);

                        multipartContent.Add(new StreamContent(contentStream), "contents", contentName);
                    }
                }

                // TODO: Add GeolocationAds and Settings if needed, similar to Contents

                // Send the request to the API
                var response = await _httpClient.PostAsync($"{this.BaseApiUri}/Add2", multipartContent);

                if (response.IsSuccessStatusCode)
                {
                    // Handle successful response
                    var responseJson = await response.Content.ReadAsStringAsync();

                    var responseData = JsonConvert.DeserializeObject<ResponseTool<Advertisement>>(responseJson);

                    return responseData;
                }
                else
                {
                    // Handle failure
                    var failResponse = ResponseFactory<Advertisement>.BuildFail("Bad Request.", null);

                    return failResponse;
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                var failResponse = ResponseFactory<Advertisement>.BuildFail($"An error occurred: {ex.Message}", null);

                return failResponse;
            }
        }

        //public async Task<Stream> GetVideoContentAsync(int id, string range)
        //{
        //    // Send an HTTP GET request to the "all" endpoint of your API
        //    HttpResponseMessage response = await this._httpClient.GetAsync($"{this.BaseApiUri}/{nameof(GetVideoContentAsync)}/{id}");

        //    // Ensure the request was successful
        //    response.EnsureSuccessStatusCode();

        //    // Read the response content as a string
        //    //string responseContent = await response.Content.ReadAsByteArrayAsync();

        //    byte[] videoBytes = await response.Content.ReadAsByteArrayAsync();

        //    if (string.IsNullOrEmpty(range))
        //    {
        //        return new MemoryStream(videoBytes);
        //    }

        //    var rangeParts = range.Replace("bytes=", "").Split('-');
        //    long start = Convert.ToInt64(rangeParts[0]);
        //    long end = rangeParts.Length > 1 ? Convert.ToInt64(rangeParts[1]) : videoBytes.Length - 1;

        //    return new MemoryStream(videoBytes, (int)start, (int)(end - start + 1));
        //}

        //public async Task<Stream> GetContentVideoAsync(int id, string range)
        //{
        //    // Send an HTTP GET request to the appropriate endpoint of your API
        //    HttpResponseMessage response = await this._httpClient.GetAsync($"{this.BaseApiUri}/{nameof(GetContentVideoAsync).Replace("Async","")}/{id}");

        //    // Ensure the request was successful
        //    response.EnsureSuccessStatusCode();

        //    // Read the response content as a byte array
        //    byte[] videoBytes = await response.Content.ReadAsByteArrayAsync();

        //    if (string.IsNullOrEmpty(range))
        //    {
        //        // If no range is specified, return the entire video content
        //        return new MemoryStream(videoBytes);
        //    }

        //    // Parse the 'Range' header: "bytes=200-1000"
        //    var rangeParts = range.Replace("bytes=", "").Split('-');

        //    long start = Convert.ToInt64(rangeParts[0]);

        //    long end = rangeParts.Length > 1 ? Convert.ToInt64(rangeParts[1]) : videoBytes.Length - 1;

        //    // Return the specified range of the video content
        //    return new MemoryStream(videoBytes, (int)start, (int)(end - start + 1));
        //}

        //public async Task<Stream> GetContentVideoAsync(int id, string range)
        //{
        //    // Create the request message
        //    var requestUri = $"{this.BaseApiUri}/{nameof(GetContentVideoAsync).Replace("Async", "")}/{id}";
        //    using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

        //    // Add Range header if specified
        //    if (!string.IsNullOrEmpty(range))
        //    {
        //        request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(
        //            long.Parse(range.Split('-')[0]), // start of range
        //            (range.Contains('-') && range.Split('-')[1] != "") ? (long?)long.Parse(range.Split('-')[1]) : null // end of range, if specified
        //        );
        //    }

        //    // Send the HTTP request
        //    using HttpResponseMessage response = await _httpClient.SendAsync(request);

        //    // Ensure the request was successful
        //    response.EnsureSuccessStatusCode();

        //    // Read the response content as a byte array
        //    byte[] videoBytes = await response.Content.ReadAsByteArrayAsync();

        //    // If no range was specified in the input, return the entire video content
        //    if (string.IsNullOrEmpty(range))
        //    {
        //        return new MemoryStream(videoBytes);
        //    }

        //    // Parse the 'Range' header: "bytes=200-1000"
        //    var rangeParts = range.Replace("bytes=", "").Split('-');

        //    long start = Convert.ToInt64(rangeParts[0]);

        //    long end = rangeParts.Length > 1 ? Convert.ToInt64(rangeParts[1]) : videoBytes.Length - 1;

        //    // Return the specified range of the video content
        //    return new MemoryStream(videoBytes, (int)start, (int)(end - start + 1));
        //}

        public async Task<byte[]> GetContentVideoAsync(int id, string range)
        {
            // Create the request message
            var requestUri = $"{this.BaseApiUri}/{nameof(GetContentVideoAsync).Replace("Async", "")}/{id}";

            using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            // Add Range header if specified
            if (!string.IsNullOrEmpty(range))
            {
                range = range.Replace("bytes=", ""); // Strip the 'bytes=' prefix if present

                long start = long.Parse(range.Split('-')[0]); // Parse start of range

                long? end = (range.Contains('-') && range.Split('-')[1] != "") ? long.Parse(range.Split('-')[1]) : null; // Parse end of range if specified

                // Set the range header
                request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(start, end);
            }

            // Send the HTTP request
            using HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            // Read the response content as a string
            var responseContent = await response.Content.ReadAsByteArrayAsync();

            // Return the response content as a stream
            return responseContent;
        }

        public async Task<ResponseTool<string>> GetStreamingVideoUrl(int id)
        {
            try
            {
                // Send an HTTP GET request to the "StreamingVideo" endpoint of your API
                HttpResponseMessage response = await _httpClient.GetAsync($"{BaseApiUri}/StreamingVideo/{id}");

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Read the response content as a string
                string videoUrl = await response.Content.ReadAsStringAsync();

                // If needed, adjust the deserialization based on the actual response
                var result = JsonConvert.DeserializeObject<ResponseTool<string>>(videoUrl);

                return result ?? ResponseFactory<string>.BuildFail("Video not found", null);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the request
                var failResponse = ResponseFactory<string>.BuildFail($"An error occurred: {ex.Message}", null);

                return failResponse;
            }
        }

        private string GetMediaType(ContentVisualType type)
        {
            return type switch
            {
                ContentVisualType.Image => "image/jpeg", // assuming jpeg, change as necessary
                ContentVisualType.Video => "video/mp4", // assuming mp4, change as necessary
                _ => "application/octet-stream"
            };
        }

        //public async Task DownloadVideoAsync(string outputPath)
        //{
        //    try
        //    {
        //        // Obtener el tamaño total del contenido
        //        var totalSize = await GetContentLengthAsync();

        //        if (totalSize == null)
        //        {
        //            Console.WriteLine("No se pudo obtener el tamaño del contenido.");
        //            return;
        //        }

        //        using (var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
        //        {
        //            for (int start = 0; start < totalSize; start += ConstantsTools.SegmentSize)
        //            {
        //                int end = start + ConstantsTools.SegmentSize - 1;

        //                if (end >= totalSize)
        //                {
        //                    end = (totalSize ?? 0) - 1;
        //                }

        //                // Descargar el segmento
        //                var buffer = await DownloadSegmentAsync(start, end);

        //                if (buffer != null)
        //                {
        //                    // Escribir en archivo
        //                    await fileStream.WriteAsync(buffer, 0, buffer.Length);

        //                    Console.WriteLine($"Segmento {start}-{end} descargado y escrito.");
        //                }
        //                else
        //                {
        //                    Console.WriteLine($"Error descargando el segmento {start}-{end}.");
        //                    return;
        //                }
        //            }
        //        }

        //        Console.WriteLine("Descarga completada con éxito.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error durante la descarga: {ex.Message}");
        //    }
        //}

        //private async Task<byte[]> DownloadSegmentAsync(int start, int end)
        //{
        //    try
        //    {
        //        var request = new HttpRequestMessage(HttpMethod.Get, _url);

        //        request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(start, end);

        //        var response = await _httpClient.SendAsync(request);

        //        response.EnsureSuccessStatusCode();

        //        return await response.Content.ReadAsByteArrayAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error al descargar el segmento {start}-{end}: {ex.Message}");

        //        return null;
        //    }
        //}

        //private async Task<int?> GetContentLengthAsync()
        //{
        //    var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, _url));

        //    if (response.IsSuccessStatusCode && response.Content.Headers.ContentLength.HasValue)
        //    {
        //        return (int)response.Content.Headers.ContentLength.Value;
        //    }

        //    return null;
        //}
    }
}