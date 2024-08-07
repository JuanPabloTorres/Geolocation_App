using GeolocationAdsAPI.ApiTools;
using GeolocationAdsAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using ToolsLibrary.Extensions;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdvertisementController : ControllerBase
    {
        private readonly string _serviceIP;

        private readonly string _servicePort;

        private readonly string _globalLocalBackendUrl;

        private readonly IAdvertisementRepository advertisementRepository;

        private readonly IContentTypeRepository contentTypeRepository;

        public AdvertisementController(IAdvertisementRepository advertisementRepository, IContentTypeRepository contentTypeRepository, IConfiguration configuration)
        {
            this.advertisementRepository = advertisementRepository;

            this.contentTypeRepository = contentTypeRepository;

            _serviceIP = configuration["ApplicationSettings:ServiceIPAddress"];

            _servicePort = configuration["ApplicationSettings:ServicePort"];

            //_serviceIP = configuration["ApplicationSettings:ServiceIPAddress2"];

            //_servicePort = configuration["ApplicationSettings:backendUrl"];

            _globalLocalBackendUrl = configuration["ApplicationSettings:GlobalLocalBackendUrl"];
        }

        [HttpPost("Add2")]
        [RequestFormLimits(MultipartBodyLengthLimit = ConstantsTools.MaxRequestBodySize)]
        [RequestSizeLimit(ConstantsTools.MaxRequestBodySize)]
        public async Task<IActionResult> AddAdvertisement()
        {
            if (!Request.HasFormContentType)
            {
                return BadRequest("Unsupported media type");
            }

            IFormCollection formCollection;

            try
            {
                formCollection = await Request.ReadFormAsync();
            }
            catch (InvalidDataException ide)
            {
                return BadRequest($"Form data is too large: {ide.Message}");
            }

            if (!formCollection.ContainsKey("advertisementMetadata"))
            {
                return BadRequest("Advertisement metadata is required.");
            }

            var advertisementJson = formCollection["advertisementMetadata"];

            Advertisement advertisement;

            try
            {
                advertisement = JsonConvert.DeserializeObject<Advertisement>(advertisementJson);

                if (advertisement == null)
                {
                    return BadRequest("Invalid advertisement data.");
                }
            }
            catch (JsonException je)
            {
                return BadRequest($"Invalid JSON data: {je.Message}");
            }

            try
            {
                foreach (var formFile in formCollection.Files)
                {
                    if (formFile.Length == 0)
                    {
                        continue; // Skip empty files
                    }

                    if (formFile.Length > ConstantsTools.MaxFileSize)
                    {
                        return BadRequest($"File size exceeds the limit: {formFile.FileName}");
                    }

                    // Instead of converting to a byte array, we can directly use the stream for further processing if needed
                    using (var memoryStream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(memoryStream);

                        // Reset the position of the MemoryStream to the beginning for any further reading
                        memoryStream.Position = 0;

                        // Extract metadata from headers if available or use default values
                        var filePath = formFile.Headers["FilePath"].FirstOrDefault();  // Correct usage

                        var createdBy = formFile.Headers["CreatedBy"].FirstOrDefault();  // Correct usage

                        //var fileSize = formFile.Headers["FilePath"].FirstOrDefault();  // Correct usage

                        // Create a ContentType object and associate it with the advertisement
                        var content = new ContentType
                        {
                            Content = memoryStream.ToArray(), // Still converting to array for ContentType object usage
                            FilePath = filePath, // Directly using the FileName here, adjust as necessary
                            FileSize = formFile.Length,
                            ContentName = formFile.FileName,
                            Type = ApiCommonsTools.DetermineContentType(formFile.ContentType),
                            CreateDate = DateTime.Now,
                            CreateBy = Convert.ToInt32(createdBy)
                        };

                        // Process the content further as needed, e.g., add to a list, etc.
                        advertisement.Contents.Add(content);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing files: {ex.Message}");
            }

            try
            {
                var response = await advertisementRepository.CreateAsync(advertisement);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        //private ContentVisualType DetermineContentType(string mimeType)
        //{
        //    // Placeholder for actual implementation
        //    switch (mimeType.ToLower())
        //    {
        //        case "image/jpeg":
        //        case "image/png":
        //        case "image/gif":
        //            return ContentVisualType.Image;

        //        case "video/mp4":
        //        case "video/mpeg":
        //            return ContentVisualType.Video;

        //        default:
        //            return ContentVisualType.Unknown;
        //    }
        //}

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ResponseTool<Advertisement> response;

            try
            {
                response = await this.advertisementRepository.Get(id);

                if (!response.IsSuccess)
                {
                    response = ResponseFactory<Advertisement>.BuildFail(response.Message, null, ToolsLibrary.Tools.Type.Fail);

                    return Ok(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<Advertisement>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        [HttpGet("[action]/{userId}/{typeId}/{pageIndex}")]
        public async Task<IActionResult> GetAdvertisementsOfUser(int userId, int typeId, int pageIndex)
        {
            ResponseTool<IEnumerable<Advertisement>> response;

            try
            {
                response = await this.advertisementRepository.GetAdvertisementsOfUser(userId, typeId, pageIndex);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<IEnumerable<Advertisement>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            ResponseTool<IEnumerable<Advertisement>> response;

            try
            {
                response = await this.advertisementRepository.GetAllAsync();

                if (!response.Data.IsObjectNull())
                {
                    response.Data = response.Data.OrderBy(o => o.CreateDate).Reverse();
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<IEnumerable<Advertisement>>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            ResponseTool<Advertisement> response;

            try
            {
                response = await this.advertisementRepository.Remove(id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<Advertisement>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
        }

        [HttpGet("[action]/{contentId}")]
        public async Task<IActionResult> GetStreamingVideoUrl(int contentId)
        {
            try
            {
                var responseResult = await contentTypeRepository.GetContentById(contentId); // Método para obtener los bytes del video de la base de datos

                if (responseResult.Data.IsObjectNull())
                {
                    return Ok(ResponseFactory<string>.BuildFail("Data Not Found", null, ToolsLibrary.Tools.Type.NotFound));
                }

                //byte[] videoBytes = responseResult.Data;

                byte[] videoBytes = ApiCommonsTools.Combine(responseResult.Data);

                var videoFile = SaveBytesToFile(videoBytes);

                var hlsOutput = ConvertVideoToHLS3(videoFile);

                // Cleanup: Elimina el archivo de video temporal
                System.IO.File.Delete(videoFile);

                var _response = ResponseFactory<string>.BuildSuccess("Streaming Path", $"{Request.Scheme}://{GetServiceEndpoint()}{hlsOutput.Replace("wwwroot", "")}");

                //return Ok($"{Request.Scheme}://{Request.Host}/{hlsOutput}");

                return Ok(_response);
            }
            catch (Exception ex)
            {
                return Ok(ResponseFactory<string>.BuildFail(ex.Message, string.Empty, ToolsLibrary.Tools.Type.Exception));
            }
        }

        // Method to get combined IP and port
        public string GetServiceEndpoint()
        {
            string ip = _serviceIP;

            string port = _servicePort;

            return $"{ip}:{port}";
        }

        //public string GetServiceEndpoint()
        //{
        //    //string ip = _serviceIP;

        //    //string port = _servicePort;

        //    return _globalLocalBackendUrl;
        //}

        //[HttpPut("[action]/{Id}")]
        //public async Task<IActionResult> Update(Advertisement advertisement, int Id)
        //{
        //    ResponseTool<Advertisement> response;

        //    try
        //    {
        //        response = await this.advertisementRepository.UpdateAsync(Id, advertisement);

        //        if (!response.IsSuccess)
        //        {
        //            response = ResponseFactory<Advertisement>.BuildFail(response.Message, null, ToolsLibrary.Tools.Type.Exception);

        //            return Ok(response);
        //        }

        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response = ResponseFactory<Advertisement>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

        //        return Ok(response);
        //    }
        //}

        [HttpPut("Update2/{Id}")]
        [RequestFormLimits(MultipartBodyLengthLimit = ConstantsTools.MaxRequestBodySize)]
        [RequestSizeLimit(ConstantsTools.MaxRequestBodySize)]
        public async Task<IActionResult> Update2(int Id)
        {
            if (!Request.HasFormContentType)
            {
                return BadRequest("Unsupported media type");
            }

            IFormCollection formCollection;

            var formDataResponse = await ApiCommonsTools.GetFormDataFromHttpRequest(Request);

            if (!formDataResponse.IsSuccess)
            {
                return Ok(formDataResponse);
            }

            formCollection = formDataResponse.Data;

            if (!formCollection.ContainsKey("advertisementMetadata"))
            {
                return BadRequest("Advertisement metadata is required.");
            }

            var advertisementJson = formCollection["advertisementMetadata"];

            Advertisement advertisement;

            try
            {
                advertisement = JsonConvert.DeserializeObject<Advertisement>(advertisementJson);

                if (advertisement == null)
                {
                    return BadRequest("Invalid advertisement data.");
                }
            }
            catch (JsonException je)
            {
                return BadRequest($"Invalid JSON data: {je.Message}");
            }

            try
            {
                foreach (var formFile in formCollection.Files)
                {
                    if (formFile.Length > ConstantsTools.MaxFileSize)
                    {
                        return BadRequest($"File size exceeds the limit: {formFile.FileName}");
                    }

                    // We use the stream directly for further processing if needed
                    using (var memoryStream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(memoryStream);

                        // Reset the position of the MemoryStream to the beginning for any further reading
                        memoryStream.Position = 0;

                        // Extract metadata from headers if available or use default values
                        var filePath = formFile.Headers["FilePath"].FirstOrDefault();  // Correct usage

                        var id = formFile.Headers["ID"].FirstOrDefault();  // Correct usage

                        var adId = formFile.Headers["AdId"].FirstOrDefault();  // Correct usage

                        // Create a ContentType object and associate it with the advertisement
                        var content = new ContentType
                        {
                            Content = memoryStream.ToArray(), // Still converting to array for ContentType object usage
                            FilePath = filePath,
                            FileSize = formFile.Length,
                            ContentName = formFile.FileName,
                            Type = ApiCommonsTools.DetermineContentType(formFile.ContentType),
                            CreateDate = DateTime.Now,
                            ID = Convert.ToInt32(id), // Assuming ID is a string; adjust the type as necessary
                            AdvertisingId = Convert.ToInt32(adId)
                        };

                        // Process the content further as needed, e.g., add to a list, etc.
                        advertisement.Contents.Add(content);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing files: {ex.Message}");
            }

            try
            {
                var response = await advertisementRepository.UpdateAsync(Id, advertisement);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        private string ConvertVideoToHLS3(string videoFilePath)
        {
            // Assuming wwwroot exists at the root of the web project and 'hls' is a folder inside it for videos.
            var outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "hls", Guid.NewGuid().ToString());

            Directory.CreateDirectory(outputDirectory);

            var outputFilePath = Path.Combine(outputDirectory, "output.m3u8");
            var startInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-i \"{videoFilePath}\" -codec: copy -start_number 0 -hls_time 10 -hls_list_size 0 -f hls \"{outputFilePath}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(startInfo))
            {
                process.WaitForExit();
            }

            // Return the relative path to the wwwroot directory.
            return outputFilePath.Substring(Directory.GetCurrentDirectory().Length).Replace("\\", "/").Replace("//", "/");
        }

        private string SaveBytesToFile(byte[] videoBytes)
        {
            var tempFilePath = Path.GetTempFileName();

            System.IO.File.WriteAllBytes(tempFilePath, videoBytes);

            return tempFilePath;
        }
    }
}