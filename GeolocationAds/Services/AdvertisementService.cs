using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
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

        //public override async Task<ResponseTool<Advertisement>> Add(Advertisement data)
        //{
        //    //try
        //    //{
        //    //    using var multipartContent = new MultipartFormDataContent();

        // // var _videosAndImage = data.Contents.Where(v => v.Type == ContentVisualType.Image ||
        // v.Type == ContentVisualType.Video).ToList();

        // // foreach (var item in _videosAndImage) // { // data.Contents.Remove(item); // } // var
        // advertisementMetadata = JsonConvert.SerializeObject(data, new JsonSerializerSettings // {
        // // ReferenceLoopHandling = ReferenceLoopHandling.Ignore // }); //
        // multipartContent.Add(new StringContent(advertisementMetadata, Encoding.UTF8,
        // "application/json"), "advertisementMetadata");

        // // try // { // //foreach (var content in _videosAndImage) // //{ // // if
        // (!string.IsNullOrEmpty(content.FilePath) && content.Content != null &&
        // content.Content.Length > 0 && (content.Type == ContentVisualType.Image || content.Type ==
        // ContentVisualType.Video)) // // { // // var fileInfo = new FileInfo(content.FilePath); //
        // // if (fileInfo.Length > ConstantsTools.MaxFileSize) // // { // // throw new
        // InvalidOperationException($"File size {fileInfo.Length} exceeds the limit of
        // {ConstantsTools.MaxFileSize} bytes."); // // }

        // // // var memoryStream = new MemoryStream();

        // // // using (var fileStream = new FileStream(content.FilePath, FileMode.Open,
        // FileAccess.Read, FileShare.Read)) // // { // // await
        // fileStream.CopyToAsync(memoryStream); // // } // // memoryStream.Position = 0; // Reset
        // the position after copying

        // // // //var byteContent = new ByteArrayContent(content.Content);

        // // // //byteContent.Headers.ContentType = new MediaTypeHeaderValue(GetMediaType(content.Type));

        // // // var contentStream = new StreamContent(memoryStream) // // { // // Headers = {
        // ContentType = new MediaTypeHeaderValue(GetMediaType(content.Type)) } // // };

        // // // contentStream.Headers.Add("ContentName", content.ContentName);

        // // // contentStream.Headers.Add("FilePath", content.FilePath);

        // // // contentStream.Headers.Add("FileSize", content.Content.Length.ToString());

        // // // contentStream.Headers.Add("CreatedBy", data.UserId.ToString()); // Assuming 'ID' is
        // a property of the content object

        // // // multipartContent.Add(contentStream, "contents", content.ContentName ?? "file"); //
        // // } // //}

        // // foreach (var content in _videosAndImage) // { // if
        // (!string.IsNullOrEmpty(content.FilePath) && content.Content != null &&
        // content.Content.Length > 0 && // (content.Type == ContentVisualType.Image || content.Type
        // == ContentVisualType.Video)) // { // Stream fileStream; // // Detectamos si es un recurso
        // embebido (está dentro del assembly) // if
        // (content.FilePath.StartsWith("GeolocationAds.Resources")) // { // var resourceStream =
        // Assembly.GetExecutingAssembly().GetManifestResourceStream(content.FilePath); // if
        // (resourceStream == null) // throw new FileNotFoundException($"Embedded resource not
        // found: {content.FilePath}");

        // // fileStream = resourceStream; // } // else // { // // Es un archivo físico en disco //
        // if (!System.IO.File.Exists(content.FilePath)) // throw new FileNotFoundException($"File
        // not found: {content.FilePath}");

        // // fileStream = new FileStream(content.FilePath, FileMode.Open, FileAccess.Read,
        // FileShare.Read); // }

        // // using var memoryStream = new MemoryStream();

        // // await fileStream.CopyToAsync(memoryStream);

        // // memoryStream.Position = 0;

        // // if (memoryStream.Length > ConstantsTools.MaxFileSize) // { // throw new
        // InvalidOperationException($"File size {memoryStream.Length} exceeds the limit of
        // {ConstantsTools.MaxFileSize} bytes."); // }

        // // var contentStream = new StreamContent(memoryStream) // { // Headers = { ContentType =
        // new MediaTypeHeaderValue(GetMediaType(content.Type)) } // };

        // // contentStream.Headers.Add("ContentName", content.ContentName);

        // // contentStream.Headers.Add("FilePath", content.FilePath);

        // // contentStream.Headers.Add("FileSize", memoryStream.Length.ToString());

        // // contentStream.Headers.Add("CreatedBy", data.UserId.ToString());

        // // multipartContent.Add(contentStream, "contents", content.ContentName ?? "file"); // }
        // // }

        // // var response = await _httpClient.PostAsync($"{this.BaseApiUri}/Add2", multipartContent);

        // // if (response.IsSuccessStatusCode) // { // var responseJson = await
        // response.Content.ReadAsStringAsync(); // return
        // JsonConvert.DeserializeObject<ResponseTool<Advertisement>>(responseJson); // } // else //
        // { // var errorResponse = await response.Content.ReadAsStringAsync(); // return
        // ResponseFactory<Advertisement>.BuildFail($"Bad Request: {errorResponse}", null); // } //
        // } // finally // { // } //} //catch (Exception ex) //{ // return
        // ResponseFactory<Advertisement>.BuildFail($"An error occurred: {ex.Message}", null); //}

        // var openedStreams = new List<Stream>();

        // try { using var multipartContent = new MultipartFormDataContent();

        // var _videosAndImage = data.Contents.Where(v => v.Type == ContentVisualType.Image ||
        // v.Type == ContentVisualType.Video).ToList();

        // foreach (var item in _videosAndImage) { data.Contents.Remove(item); }

        // var advertisementMetadata = JsonConvert.SerializeObject(data, new JsonSerializerSettings
        // { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

        // multipartContent.Add(new StringContent(advertisementMetadata, Encoding.UTF8,
        // "application/json"), "advertisementMetadata");

        // foreach (var content in _videosAndImage) { if (!string.IsNullOrEmpty(content.FilePath) &&
        // content.Content?.Length > 0) { Stream fileStream;

        // if (content.FilePath.StartsWith("GeolocationAds.Resources")) { fileStream =
        // Assembly.GetExecutingAssembly().GetManifestResourceStream(content.FilePath) ?? throw new
        // FileNotFoundException($"Embedded resource not found: {content.FilePath}"); } else { if
        // (!System.IO.File.Exists(content.FilePath)) throw new FileNotFoundException($"File not
        // found: {content.FilePath}");

        // fileStream = new FileStream(content.FilePath, FileMode.Open, FileAccess.Read,
        // FileShare.Read); }

        // var memoryStream = new MemoryStream(); await fileStream.CopyToAsync(memoryStream);
        // memoryStream.Position = 0;

        // // 👉 Guarda los streams abiertos para cerrarlos después openedStreams.Add(fileStream); openedStreams.Add(memoryStream);

        // var contentStream = new StreamContent(memoryStream) { Headers = { ContentType = new
        // MediaTypeHeaderValue(GetMediaType(content.Type)) } };

        // contentStream.Headers.Add("ContentName", content.ContentName);
        // contentStream.Headers.Add("FilePath", content.FilePath);
        // contentStream.Headers.Add("FileSize", memoryStream.Length.ToString());
        // contentStream.Headers.Add("CreatedBy", data.UserId.ToString());

        // multipartContent.Add(contentStream, "contents", content.ContentName ?? "file"); } }

        // var response = await _httpClient.PostAsync($"{this.BaseApiUri}/Add2", multipartContent);

        // var responseJson = await response.Content.ReadAsStringAsync();

        // return JsonConvert.DeserializeObject<ResponseTool<Advertisement>>(responseJson); } catch
        // (Exception ex) { return ResponseFactory<Advertisement>.BuildFail($"An error occurred:
        // {ex.Message}", null); }

        // finally { // ❌ Nunca cierres el stream antes de enviar foreach (var s in openedStreams)
        // s.Dispose(); }

        //}

        //public async Task<ResponseTool<IEnumerable<Advertisement>>> GetAdvertisementsOfUser(int userId, int typeId, int? pageIndex)
        //{
        //    try
        //    {
        //        // Build the full API endpoint URL for the "all" endpoint

        // // Send an HTTP GET request to the "all" endpoint of your API HttpResponseMessage
        // response = await this._httpClient.GetAsync($"{this.BaseApiUri}/{nameof(GetAdvertisementsOfUser)}/{userId}/{typeId}/{pageIndex}");

        // // Ensure the request was successful response.EnsureSuccessStatusCode();

        // // Read the response content as a string string responseContent = await response.Content.ReadAsStringAsync();

        // // Deserialize the response content to your custom ResponseTool<IEnumerable<T>> type var
        // result = JsonConvert.DeserializeObject<ResponseTool<IEnumerable<Advertisement>>>(responseContent);

        // return result; } catch (Exception ex) { // Handle any exceptions that may occur during
        // the request For simplicity, we'll // just return an error ResponseTool with the exception message

        // var failResponse = ResponseFactory<IEnumerable<Advertisement>>.BuildFail($"An error
        // occurred: {ex.Message}", null);

        //        return failResponse;
        //    }
        //}

        public override async Task<ResponseTool<Advertisement>> Add(Advertisement data)
        {
            var openedStreams = new List<Stream>();

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

                foreach (var content in _videosAndImage)
                {
                    if (!string.IsNullOrEmpty(content.FilePath) && content.Content?.Length > 0)
                    {
                        Stream fileStream;

                        if (content.FilePath.StartsWith("GeolocationAds.Resources"))
                        {
                            fileStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(content.FilePath)
                                         ?? throw new FileNotFoundException($"Embedded resource not found: {content.FilePath}");
                        }
                        else
                        {
                            if (!System.IO.File.Exists(content.FilePath))
                                throw new FileNotFoundException($"File not found: {content.FilePath}");

                            fileStream = new FileStream(content.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        }

                        var memoryStream = new MemoryStream();

                        await fileStream.CopyToAsync(memoryStream);

                        memoryStream.Position = 0;

                        openedStreams.Add(fileStream);

                        openedStreams.Add(memoryStream);

                        var contentStream = new StreamContent(memoryStream)
                        {
                            Headers = { ContentType = new MediaTypeHeaderValue(GetMediaType(content.Type)) }
                        };

                        contentStream.Headers.Add("ContentName", content.ContentName);

                        contentStream.Headers.Add("FilePath", content.FilePath);

                        contentStream.Headers.Add("FileSize", memoryStream.Length.ToString());

                        contentStream.Headers.Add("CreatedBy", data.UserId.ToString());

                        multipartContent.Add(contentStream, "contents", content.ContentName ?? "file");
                    }
                }

                // ✅ Aquí se usa tu manejador central
                return await HandleRequest<Advertisement>(async () =>
                {
                    return await _httpClient.PostAsync($"{this.BaseApiUri}/Add2", multipartContent);
                });
            }
            catch (Exception ex)
            {
                return ResponseFactory<Advertisement>.BuildFail($"An error occurred: {ex.Message}", null);
            }
            finally
            {
                foreach (var s in openedStreams)
                    s.Dispose();
            }
        }

        public async Task<ResponseTool<IEnumerable<Advertisement>>> GetAdvertisementsOfUser(int userId, int typeId, int? pageIndex)
        {
            return await HandleRequest<IEnumerable<Advertisement>>(async () =>
            {
                var url = $"{this.BaseApiUri}/{nameof(GetAdvertisementsOfUser)}/{userId}/{typeId}/{pageIndex}";

                return await _httpClient.GetAsync(url);
            });
        }

        public async Task<ResponseTool<IEnumerable<Advertisement>>> GetAdvertisementsOfUserStreamedAsync(int userId, int typeId, int? pageIndex)
        {
            return await HandleRequest<IEnumerable<Advertisement>>(
                async () =>
                {
                    var url = $"{this.BaseApiUri}/{nameof(GetAdvertisementsOfUser)}/{userId}/{typeId}/{pageIndex}";
                    return await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                },
                async response =>
                {
                    await using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                    using var reader = new StreamReader(stream);

                    using var jsonReader = new JsonTextReader(reader);

                    var serializer = new JsonSerializer();

                    var wrapper = serializer.Deserialize<ResponseTool<List<Advertisement>>>(jsonReader);

                    if (wrapper == null || !wrapper.IsSuccess || wrapper.Data == null)
                    {
                        return ResponseFactory<IEnumerable<Advertisement>>.BuildFail(
                            wrapper?.Message ?? "Empty or invalid response.",
                            null,
                            ToolsLibrary.Tools.Type.Fail);
                    }

                    if (!wrapper.Data.Any())
                    {
                        return ResponseFactory<IEnumerable<Advertisement>>.BuildFail(
                            "No advertisements found.",
                            Enumerable.Empty<Advertisement>(),
                            ToolsLibrary.Tools.Type.NotFound);
                    }

                    return ResponseFactory<IEnumerable<Advertisement>>.BuildSuccess(
                        "Advertisements streamed and parsed successfully.",
                        wrapper.Data,
                        ToolsLibrary.Tools.Type.DataFound);
                });
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
        //    try
        //    {
        //        using var multipartContent = new MultipartFormDataContent();

        // var _videosAndImage = data.Contents.Where(v => v.Type == ContentVisualType.Image ||
        // v.Type == ContentVisualType.Video).ToList();

        // // Removing video and image content from the main data object to prevent serialization
        // data.Contents = data.Contents.Except(_videosAndImage).ToList();

        // // Serialize the advertisement data without the image and video content var
        // advertisementMetadata = JsonConvert.SerializeObject(data, new JsonSerializerSettings {
        // ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

        // multipartContent.Add(new StringContent(advertisementMetadata, Encoding.UTF8,
        // "application/json"), "advertisementMetadata");

        // // Add media files to the multipart form data with additional metadata foreach (var
        // content in _videosAndImage) { if (content.Content != null) { var memoryStream = new MemoryStream(content.Content);

        // var contentStream = new StreamContent(memoryStream) { Headers = { ContentType = new
        // MediaTypeHeaderValue(GetMediaType(content.Type)) } };

        // // Adding additional headers to include metadata contentStream.Headers.ContentDisposition
        // = new ContentDispositionHeaderValue("form-data") { Name = "contents", FileName =
        // content.ContentName ?? "file" }; contentStream.Headers.Add("ContentName", content.ContentName);

        // contentStream.Headers.Add("FilePath", content.FilePath);

        // contentStream.Headers.Add("FileSize", content.FileSize.ToString());

        // contentStream.Headers.Add("ID", content.ID.ToString()); // Assuming 'ID' is a property of
        // the content object

        // contentStream.Headers.Add("AdId", data.ID.ToString()); // Assuming 'ID' is a property of
        // the content object

        // multipartContent.Add(contentStream, "contents", content.ContentName ?? "file"); } }

        // var response = await _httpClient.PutAsync($"{this.BaseApiUri}/Update2/{currentId}", multipartContent);

        // if (response.IsSuccessStatusCode) { var responseJson = await response.Content.ReadAsStringAsync();

        // return JsonConvert.DeserializeObject<ResponseTool<Advertisement>>(responseJson); } else {
        // var errorResponse = await response.Content.ReadAsStringAsync();

        //            return ResponseFactory<Advertisement>.BuildFail($"Bad Request: {errorResponse}", null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ResponseFactory<Advertisement>.BuildFail($"An error occurred: {ex.Message}", null);
        //    }
        //}

        //public override async Task<ResponseTool<Advertisement>> Update(Advertisement data, int currentId)
        //{
        //    var openedStreams = new List<Stream>();

        // try { using var multipartContent = new MultipartFormDataContent();

        // var _videosAndImage = data.Contents .Where(v => v.Type == ContentVisualType.Image ||
        // v.Type == ContentVisualType.Video) .ToList();

        // data.Contents = data.Contents.Except(_videosAndImage).ToList();

        // var advertisementMetadata = JsonConvert.SerializeObject(data, new JsonSerializerSettings
        // { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

        // multipartContent.Add(new StringContent(advertisementMetadata, Encoding.UTF8,
        // "application/json"), "advertisementMetadata");

        // foreach (var content in _videosAndImage) { if (content.Content != null) { var
        // memoryStream = new MemoryStream(content.Content); openedStreams.Add(memoryStream);

        // var contentStream = new StreamContent(memoryStream) { Headers = { ContentType = new
        // MediaTypeHeaderValue(GetMediaType(content.Type)) } };

        // contentStream.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        // { Name = "contents", FileName = content.ContentName ?? "file" };

        // contentStream.Headers.Add("ContentName", content.ContentName);
        // contentStream.Headers.Add("FilePath", content.FilePath);
        // contentStream.Headers.Add("FileSize", content.FileSize.ToString());
        // contentStream.Headers.Add("ID", content.ID.ToString()); contentStream.Headers.Add("AdId", data.ID.ToString());

        // multipartContent.Add(contentStream, "contents", content.ContentName ?? "file"); } }

        //        return await HandleRequest<Advertisement>(async () =>
        //        {
        //            return await _httpClient.PutAsync($"{this.BaseApiUri}/Update2/{currentId}", multipartContent);
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return ResponseFactory<Advertisement>.BuildFail($"An error occurred: {ex.Message}", null);
        //    }
        //    finally
        //    {
        //        foreach (var s in openedStreams)
        //            s.Dispose();
        //    }
        //}

        public override async Task<ResponseTool<Advertisement>> Update(Advertisement data, int currentId)
        {
            try
            {
                using var multipartContent = new MultipartFormDataContent();

                var _videosAndImage = data.Contents
                    .Where(v => v.Type == ContentVisualType.Image || v.Type == ContentVisualType.Video)
                    .ToList();

                // Removemos los medios pesados del objeto para serialización más liviana
                data.Contents = data.Contents.Except(_videosAndImage).ToList();

                var advertisementMetadata = JsonConvert.SerializeObject(data, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                multipartContent.Add(new StringContent(advertisementMetadata, Encoding.UTF8, "application/json"), "advertisementMetadata");

                foreach (var content in _videosAndImage)
                {
                    if (content.Content != null)
                    {
                        var byteContent = new ByteArrayContent(content.Content);

                        byteContent.Headers.ContentType = new MediaTypeHeaderValue(GetMediaType(content.Type));

                        byteContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "contents",
                            FileName = content.ContentName ?? "file"
                        };

                        byteContent.Headers.Add("ContentName", content.ContentName);
                        byteContent.Headers.Add("FilePath", content.FilePath);
                        byteContent.Headers.Add("FileSize", content.FileSize.ToString());
                        byteContent.Headers.Add("ID", content.ID.ToString());
                        byteContent.Headers.Add("AdId", data.ID.ToString());

                        multipartContent.Add(byteContent, "contents", content.ContentName ?? "file");

                        // 🔽 Libera el arreglo de bytes después de usarlo (si no será reutilizado)
                        content.Content = null;
                    }
                }

                return await HandleRequest<Advertisement>(async () =>
                {
                    return await _httpClient.PutAsync($"{this.BaseApiUri}/Update2/{currentId}", multipartContent);
                });
            }
            catch (Exception ex)
            {
                return ResponseFactory<Advertisement>.BuildFail($"An error occurred: {ex.Message}", null);
            }
        }
    }
}