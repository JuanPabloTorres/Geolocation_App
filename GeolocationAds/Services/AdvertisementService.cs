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

                            //var byteContent = new ByteArrayContent(content.Content);

                            //byteContent.Headers.ContentType = new MediaTypeHeaderValue(GetMediaType(content.Type));

                            var contentStream = new StreamContent(memoryStream)
                            {
                                Headers = { ContentType = new MediaTypeHeaderValue(GetMediaType(content.Type)) }
                            };

                            contentStream.Headers.Add("ContentName", content.ContentName);

                            contentStream.Headers.Add("FilePath", content.FilePath);

                            contentStream.Headers.Add("FileSize", content.Content.Length.ToString());

                            contentStream.Headers.Add("CreatedBy", data.UserId.ToString()); // Assuming 'ID' is a property of the content object

                            multipartContent.Add(contentStream, "contents", content.ContentName ?? "file");
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
                // Handle any exceptions that may occur during the request For simplicity, we'll
                // just return an error ResponseTool with the exception message

                var failResponse = ResponseFactory<IEnumerable<Advertisement>>.BuildFail($"An error occurred: {ex.Message}", null);

                return failResponse;
            }
        }

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

        //public override async Task<ResponseTool<Advertisement>> Update(Advertisement data, int currentId)
        //{
        //    // Serialize the data object to JSON
        //    var json = JsonConvert.SerializeObject(data);

        // var content = new StringContent(json, Encoding.UTF8, "application/json");

        // // Construct the API endpoint URL for the specific resource and ID var apiUrl = $"{this.BaseApiUri}/update/{currentId}";

        // try { // Execute the PUT request to the API var response = await
        // _httpClient.PutAsync(apiUrl, content);

        // // Process the API response if (response.IsSuccessStatusCode) { // Parse the successful
        // response var responseJson = await response.Content.ReadAsStringAsync();

        // var responseData = JsonConvert.DeserializeObject<ResponseTool<Advertisement>>(responseJson);

        // return responseData; } else { // Log and handle non-success status var errorContent =
        // await response.Content.ReadAsStringAsync();

        //            // Optionally log errorContent to your logging framework
        //            return ResponseFactory<Advertisement>.BuildFail($"API request failed with status: {response.StatusCode}", null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions from HTTP request or processing
        //        // Optionally log ex to your logging framework
        //        return ResponseFactory<Advertisement>.BuildFail($"An error occurred: {ex.Message}", null);
        //    }
        //}

        public override async Task<ResponseTool<Advertisement>> Update(Advertisement data, int currentId)
        {
            try
            {
                using var multipartContent = new MultipartFormDataContent();

                var _videosAndImage = data.Contents.Where(v => v.Type == ContentVisualType.Image || v.Type == ContentVisualType.Video).ToList();

                // Removing video and image content from the main data object to prevent serialization
                data.Contents = data.Contents.Except(_videosAndImage).ToList();

                // Serialize the advertisement data without the image and video content
                var advertisementMetadata = JsonConvert.SerializeObject(data, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                multipartContent.Add(new StringContent(advertisementMetadata, Encoding.UTF8, "application/json"), "advertisementMetadata");

                // Add media files to the multipart form data with additional metadata
                foreach (var content in _videosAndImage)
                {
                    if (content.Content != null)
                    {
                        var memoryStream = new MemoryStream(content.Content);

                        var contentStream = new StreamContent(memoryStream)
                        {
                            Headers = { ContentType = new MediaTypeHeaderValue(GetMediaType(content.Type)) }
                        };

                        // Adding additional headers to include metadata
                        contentStream.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "contents",
                            FileName = content.ContentName ?? "file"
                        };
                        contentStream.Headers.Add("ContentName", content.ContentName);

                        contentStream.Headers.Add("FilePath", content.FilePath);

                        contentStream.Headers.Add("FileSize", content.FileSize.ToString());

                        contentStream.Headers.Add("ID", content.ID.ToString()); // Assuming 'ID' is a property of the content object

                        contentStream.Headers.Add("AdId", data.ID.ToString()); // Assuming 'ID' is a property of the content object

                        multipartContent.Add(contentStream, "contents", content.ContentName ?? "file");
                    }
                }

                var response = await _httpClient.PutAsync($"{this.BaseApiUri}/Update2/{currentId}", multipartContent);

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
            catch (Exception ex)
            {
                return ResponseFactory<Advertisement>.BuildFail($"An error occurred: {ex.Message}", null);
            }
        }

        //public override async Task<ResponseTool<Advertisement>> Update(Advertisement data, int currentId)
        //{
        //    try
        //    {
        //        using var multipartContent = new MultipartFormDataContent();

        // var mediaContents = data.Contents .Where(v => v.Type == ContentVisualType.Image || v.Type
        // == ContentVisualType.Video) .ToList();

        // // 🔹 Serializar el objeto `data` sin imágenes/videos var advertisementMetadata =
        // JsonConvert.SerializeObject(data, new JsonSerializerSettings { ReferenceLoopHandling =
        // ReferenceLoopHandling.Ignore });

        // multipartContent.Add(new StringContent(advertisementMetadata, Encoding.UTF8,
        // "application/json"), "advertisementMetadata");

        // // 🔹 Agregar archivos multimedia al request foreach (var content in mediaContents) { if
        // (content.Content != null) { var memoryStream = new MemoryStream(content.Content); // ⬅️
        // NO USAR `using` AQUÍ var contentStream = new StreamContent(memoryStream) { Headers = {
        // ContentType = new MediaTypeHeaderValue(GetMediaType(content.Type)) } };

        // contentStream.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        // { Name = "contents", FileName = content.ContentName ?? "file" };

        // contentStream.Headers.Add("ContentName", content.ContentName ?? "file");
        // contentStream.Headers.Add("FilePath", content.FilePath);
        // contentStream.Headers.Add("FileSize", content.FileSize.ToString());
        // contentStream.Headers.Add("ID", content.ID.ToString()); contentStream.Headers.Add("AdId", data.ID.ToString());

        // multipartContent.Add(contentStream, "contents", content.ContentName ?? "file"); } }

        // // 🔹 Enviar la solicitud HTTP var response = await
        // _httpClient.PutAsync($"{this.BaseApiUri}/Update2/{currentId}", multipartContent);

        //        return await HandleResponse(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ResponseFactory<Advertisement>.BuildFail($"An error occurred: {ex.Message}", null);
        //    }
        //}

        private void AddMediaToMultipartContent(MultipartFormDataContent multipartContent, ContentType content, int adId)
        {
            using var memoryStream = new MemoryStream(content.Content);

            var contentStream = new StreamContent(memoryStream)
            {
                Headers = { ContentType = new MediaTypeHeaderValue(GetMediaType(content.Type)) }
            };

            // 🔹 Asignar metadatos al archivo
            var contentHeaders = new Dictionary<string, string>
    {
        { "ContentName", content.ContentName ?? "file" },
        { "FilePath", content.FilePath },
        { "FileSize", content.FileSize.ToString() },
        { "ID", content.ID.ToString() },
        { "AdId", adId.ToString() }
    };

            foreach (var header in contentHeaders)
            {
                contentStream.Headers.Add(header.Key, header.Value);
            }

            contentStream.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "contents",
                FileName = content.ContentName ?? "file"
            };

            multipartContent.Add(contentStream, "contents", content.ContentName ?? "file");
        }

        private async Task<ResponseTool<Advertisement>> HandleResponse(HttpResponseMessage response)
        {
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
    }
}