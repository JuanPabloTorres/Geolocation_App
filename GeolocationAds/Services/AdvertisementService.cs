using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;


namespace GeolocationAds.Services
{
    public class AdvertisementService : BaseService<Advertisement>, IAdvertisementService
    {
        public AdvertisementService(HttpClient htppClient, IConfiguration configuration) : base(htppClient, configuration)
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

        //public override async Task<ResponseTool<Advertisement>> Add(Advertisement data)
        //{
        //    try
        //    {
        //        var json = JsonConvert.SerializeObject(data);

        //        var content = new StringContent(json, Encoding.UTF8, "application/json");

        //        var response = await _httpClient.PostAsync($"{this.BaseApiUri}/Add", content);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var responseJson = await response.Content.ReadAsStringAsync();

        //            var responseData = JsonConvert.DeserializeObject<ResponseTool<Advertisement>>(responseJson);

        //            return responseData;
        //        }
        //        else
        //        {
        //            var failResponse = ResponseFactory<Advertisement>.BuildFail("Bad Request.", null);

        //            return failResponse;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var failResponse = ResponseFactory<Advertisement>.BuildFail($"An error occurred: {ex.Message}", null);

        //        return failResponse;
        //    }
        //}



        public override async Task<ResponseTool<Advertisement>> Add(Advertisement data)
        {
            try
            {
                using var multipartContent = new MultipartFormDataContent();

                var _videosAndImage = data.Contents.Where(v => v.Type == ContentVisualType.Image || v.Type == ContentVisualType.Video).ToList();

                foreach (var item in _videosAndImage)
                {
                    data.Contents.Remove(item);
                }


                var advertisementMetadata = JsonConvert.SerializeObject(data, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                multipartContent.Add(new StringContent(advertisementMetadata, Encoding.UTF8, "application/json"), "advertisementMetadata");



                try
                {

                    foreach (var content in _videosAndImage)
                    {
                        //if (content.Type == ContentVisualType.URL)
                        //{
                        //    // Handle URLs as StringContent
                        //    if (!string.IsNullOrEmpty(content.Url))
                        //    {
                        //        var urlContent = new StringContent(content.Url)
                        //        {
                        //            Headers = { ContentType = new MediaTypeHeaderValue("text/plain") }  // Sending URL as plain text
                        //        };

                        //        multipartContent.Add(urlContent, "url", content.ContentName ?? "url");
                        //    }
                        //}
                        if (!string.IsNullOrEmpty(content.FilePath) && content.Content != null && content.Content.Length > 0 && (content.Type == ContentVisualType.Image || content.Type == ContentVisualType.Video))
                        {
                            var fileInfo = new FileInfo(content.FilePath);

                            if (fileInfo.Length > ConstantsTools.MaxFileSize)
                            {
                                throw new InvalidOperationException($"File size {fileInfo.Length} exceeds the limit of {ConstantsTools.MaxFileSize} bytes.");
                            }

                            var memoryStream = new MemoryStream();

                            using (var fileStream = new FileStream(content.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            {
                                await fileStream.CopyToAsync(memoryStream);
                            }
                            memoryStream.Position = 0; // Reset the position after copying

                            var byteContent = new ByteArrayContent(content.Content);

                            byteContent.Headers.ContentType = new MediaTypeHeaderValue(GetMediaType(content.Type));

                            var contentStream = new StreamContent(memoryStream)
                            {
                                Headers = { ContentType = new MediaTypeHeaderValue(GetMediaType(content.Type)) }
                            };

                            multipartContent.Add(byteContent, "contents", content.ContentName ?? "file");

                        }
                    }



                    var response = await _httpClient.PostAsync($"{this.BaseApiUri}/Add2", multipartContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseJson = await response.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<ResponseTool<Advertisement>>(responseJson);
                    }
                    else
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();

                        return ResponseFactory<Advertisement>.BuildFail($"Bad Request: {errorResponse}", null);
                    }
                }
                finally
                {

                }
            }
            catch (Exception ex)
            {
                return ResponseFactory<Advertisement>.BuildFail($"An error occurred: {ex.Message}", null);
            }
        }

        public async Task<ResponseTool<Advertisement>> UploadFileInChunks(string filePath, int chunkSize = 1048576) // Default chunk size is 1 MB
        {
            FileInfo fileInfo = new FileInfo(filePath);
            long totalChunks = (fileInfo.Length + chunkSize - 1) / chunkSize; // Calculate total number of chunks
            List<Task> uploadTasks = new List<Task>();

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[chunkSize];
                int chunkNumber = 0;
                int bytesRead;
                while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    // Ensure the last buffer is right-sized for remaining content
                    byte[] chunkToSend = buffer;
                    if (bytesRead < chunkSize)
                    {
                        chunkToSend = new byte[bytesRead];
                        Array.Copy(buffer, chunkToSend, bytesRead);
                    }

                    // Create a separate scope for the captured variables
                    int capturedChunkNumber = chunkNumber;
                    uploadTasks.Add(Task.Run(() => UploadChunk(capturedChunkNumber, chunkToSend, totalChunks)));

                    chunkNumber++;
                }
            }

            await Task.WhenAll(uploadTasks); // Wait for all chunks to be uploaded

            return new ResponseTool<Advertisement> { IsSuccess = true, Message = "File uploaded in chunks successfully." };
        }

        private async Task UploadChunk(int chunkNumber, byte[] chunk, long totalChunks)
        {
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new ByteArrayContent(chunk), "fileChunk", $"chunk_{chunkNumber}");
                // Add additional form data as needed

                var response = await _httpClient.PostAsync($"{this.BaseApiUri}/Add2", content);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle failure per your application's needs
                    throw new InvalidOperationException("Failed to upload chunk.");
                }
            }
        }

        //public override async Task<ResponseTool<Advertisement>> Add(Advertisement data)
        //{
        //    try
        //    {
        //        //this._httpClient.Timeout = TimeSpan.FromMinutes(30); // Set an appropriate timeout

        //        using var multipartContent = new MultipartFormDataContent();

        //        var _contens = data.Contents;

        //        //data.Contents.Clear();

        //        var advertisementMetadata = JsonConvert.SerializeObject(data, new JsonSerializerSettings
        //        {
        //            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //        });

        //        multipartContent.Add(new StringContent(advertisementMetadata, Encoding.UTF8, "application/json"), "advertisementMetadata");

        //        foreach (var content in _contens)
        //        {
        //            if (content.Content != null && content.Content.Length > 0)
        //            {
        //                var fileInfo = new FileInfo(content.FilePath);

        //                if (fileInfo.Length > ConstantsTools.MaxFileSize)
        //                {
        //                    throw new InvalidOperationException($"File size {fileInfo.Length} exceeds the limit.");
        //                }

        //                // Read directly from the file stream
        //                var fileStream = new FileStream(content.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);

        //                var contentStream = new StreamContent(fileStream)
        //                {
        //                    Headers = { ContentType = new MediaTypeHeaderValue(GetMediaType(content.Type)) }
        //                };

        //                multipartContent.Add(contentStream, "contents", content.ContentName ?? "file");
        //            }
        //        }

        //        var response = await _httpClient.PostAsync($"{this.BaseApiUri}/Add2", multipartContent);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var responseJson = await response.Content.ReadAsStringAsync();

        //            return JsonConvert.DeserializeObject<ResponseTool<Advertisement>>(responseJson);
        //        }
        //        else
        //        {
        //            return ResponseFactory<Advertisement>.BuildFail("Bad Request.", null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception as needed
        //        return ResponseFactory<Advertisement>.BuildFail($"An error occurred: {ex.Message}", null);
        //    }
        //}

        //public override async Task<ResponseTool<Advertisement>> Add(Advertisement data)
        //{
        //    try
        //    {
        //        using var multipartContent = new MultipartFormDataContent();

        //        // Serialize Advertisement metadata with efficient handling of reference loops
        //        var advertisementMetadata = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        //        multipartContent.Add(new StringContent(advertisementMetadata, Encoding.UTF8, "application/json"), "advertisementMetadata");

        //        // Stream content directly to reduce memory usage
        //        foreach (var content in data.Contents)
        //        {
        //            if (content.Content != null && content.Content.Length > 0)
        //            {
        //                // Use PushStreamContent to allow streaming of data directly
        //                var contentStream = new PushStreamContent(async (stream, httpContent, transportContext) =>
        //                {
        //                    using (var memoryStream = new MemoryStream(content.Content))
        //                    {
        //                        await memoryStream.CopyToAsync(stream);
        //                    }
        //                });

        //                var contentName = content.ContentName ?? "file";
        //                var mediaType = GetMediaType(content.Type);
        //                multipartContent.Add(contentStream, "contents", contentName);
        //            }
        //        }

        //        // Configure the HttpClient request to have a timeout
        //        _httpClient.Timeout = TimeSpan.FromMinutes(5);  // Adjust timeout based on expected upload size and network conditions

        //        // Send the request to the API
        //        var response = await _httpClient.PostAsync($"{this.BaseApiUri}/Add2", multipartContent);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var responseJson = await response.Content.ReadAsStringAsync();
        //            var responseData = JsonConvert.DeserializeObject<ResponseTool<Advertisement>>(responseJson);
        //            return responseData;
        //        }
        //        else
        //        {
        //            return ResponseFactory<Advertisement>.BuildFail("Bad Request.", null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error in Add operation");
        //        return ResponseFactory<Advertisement>.BuildFail($"An error occurred: {ex.Message}", null);
        //    }
        //}

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
                HttpResponseMessage response = await _httpClient.GetAsync($"{BaseApiUri}/{nameof(GetStreamingVideoUrl)}/{id}");

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
                ContentVisualType.URL => "text/html", // assuming mp4, change as necessary
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