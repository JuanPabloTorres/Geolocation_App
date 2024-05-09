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

        private readonly IAdvertisementRepository advertisementRepository;

        private readonly IContentTypeRepository contentTypeRepository;

        public AdvertisementController(IAdvertisementRepository advertisementRepository, IContentTypeRepository contentTypeRepository, IConfiguration configuration)
        {
            this.advertisementRepository = advertisementRepository;

            this.contentTypeRepository = contentTypeRepository;

            _serviceIP = configuration["ApplicationSettings:ServiceIPAddress"];

            _servicePort = configuration["ApplicationSettings:ServicePort"];
        }

        [HttpPost("[action]")]
        [RequestFormLimits(MultipartBodyLengthLimit = ConstantsTools.MaxRequestBodySize)]
        [RequestSizeLimit(ConstantsTools.MaxRequestBodySize)]
        public async Task<IActionResult> Add([FromBody] Advertisement advertisement)
        {
            ResponseTool<Advertisement> response;

            try
            {
                response = await this.advertisementRepository.CreateAsync(advertisement);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ResponseFactory<Advertisement>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

                return Ok(response);
            }
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

            advertisement.Contents = new List<ContentType>(); // Ensuring the Contents collection is initialized

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

                        // Create a ContentType object and associate it with the advertisement
                        var content = new ContentType
                        {
                            Content = memoryStream.ToArray(), // Still converting to array for ContentType object usage
                            FilePath = string.Empty, // Directly using the FileName here, adjust as necessary
                            FileSize = formFile.Length,
                            ContentName = formFile.FileName,
                            Type = DetermineContentType(formFile.ContentType)
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

        private async Task<string> SaveFileAsync(IFormFile file)
        {
            var filePath = Path.Combine("uploads", file.FileName);
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filePath;
        }

        private ContentVisualType DetermineContentType(string mimeType)
        {
            // Placeholder for actual implementation
            switch (mimeType.ToLower())
            {
                case "image/jpeg":
                case "image/png":
                case "image/gif":
                    return ContentVisualType.Image;
                case "video/mp4":
                case "video/mpeg":
                    return ContentVisualType.Video;
                default:
                    return ContentVisualType.Unknown;
            }
        }

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

                byte[] videoBytes = responseResult.Data.Content;

                var videoFile = SaveBytesToFile(videoBytes);

                var hlsOutput = ConvertVideoToHLS3(videoFile);

                // Cleanup: Elimina el archivo de video temporal
                System.IO.File.Delete(videoFile);

                //var _response = ResponseFactory<string>.BuildSusccess("Streaming Path", $"{Request.Scheme}://{Request.Host}/{hlsOutput}");

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

        [HttpPut("[action]/{Id}")]
        public async Task<IActionResult> Update(Advertisement advertisement, int Id)
        {
            ResponseTool<Advertisement> response;

            try
            {
                response = await this.advertisementRepository.UpdateAsync(Id, advertisement);

                if (!response.IsSuccess)
                {
                    response = ResponseFactory<Advertisement>.BuildFail(response.Message, null, ToolsLibrary.Tools.Type.Exception);

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

        private async Task<byte[]> ConvertToByteArray(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
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

        private ContentVisualType GetContentVisualType(string contentType)
        {
            return contentType switch
            {
                "image/jpeg" => ContentVisualType.Image,
                "image/png" => ContentVisualType.Image,
                "video/mp4" => ContentVisualType.Video,
                _ => ContentVisualType.Unknown
            };
        }

        //[HttpGet("[action]/{id}")]
        //public async Task<IActionResult> StreamingContent(int id)
        //{
        //    var responseResult = await contentTypeRepository.GetContentById(id); // Método para obtener los bytes del video de la base de datos

        //    byte[] videoBytes = responseResult.Data.Content;

        //    // Lee la cabecera 'Range' enviada por el cliente
        //    HttpContext.Request.Headers.TryGetValue("Range", out StringValues range);

        //    if (StringValues.IsNullOrEmpty(range))
        //    {
        //        // Si no hay rango especificado, envía todo el contenido
        //        return File(videoBytes, "video/mp4");
        //    }

        //    // Parsea la cabecera 'Range': "bytes=200-1000"
        //    var rangeString = range.ToString().Replace("bytes=", "").Split('-');

        //    long start = Convert.ToInt64(rangeString[0]);

        //    long end = (rangeString.Length > 1) ? Convert.ToInt64(rangeString[1]) : videoBytes.Length - 1;

        //    MemoryStream memoryStream = new MemoryStream(videoBytes, (int)start, (int)(end - start + 1));

        //    // Establece el código de estado y la cabecera 'Content-Range'
        //    Response.StatusCode = 206; // Partial Content

        //    Response.Headers.Add("Content-Range", $"bytes {start}-{end}/{videoBytes.Length}");

        //    return new FileStreamResult(memoryStream, "video/mp4");
        //}
        private string SaveBytesToFile(byte[] videoBytes)
        {
            var tempFilePath = Path.GetTempFileName();

            System.IO.File.WriteAllBytes(tempFilePath, videoBytes);

            return tempFilePath;
        }

        //[HttpGet("[action]/{id}")]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetContentVideo(int id)
        //{
        //    ResponseTool<byte[]> response;
        //    try
        //    {
        //        // Lee la cabecera 'Range' enviada por el cliente
        //        HttpContext.Request.Headers.TryGetValue("Range", out StringValues range);

        //        var responseResult = await contentTypeRepository.GetContentById(id); // Método para obtener los bytes del video de la base de datos

        //        if (StringValues.IsNullOrEmpty(range))
        //        {
        //            // Parsea la cabecera 'Range': "bytes=200-1000"
        //            var rangeString = range.ToString().Replace("bytes=", "").Split('-');

        //            int start = Convert.ToInt32(rangeString[0]);

        //            int end = (rangeString.Length > 1) ? Convert.ToInt32(rangeString[1]) : responseResult.Data.Content.Length - 1;

        //            byte[] videoBytes = responseResult.Data.Content.Skip(start).Take(end).ToArray();

        //            response = ResponseFactory<byte[]>.BuildSusccess("Segment", videoBytes, ToolsLibrary.Tools.Type.Succesfully);

        //            return Ok(response);
        //        }

        //        response = ResponseFactory<byte[]>.BuildSusccess("Segment", responseResult.Data.Content, ToolsLibrary.Tools.Type.Succesfully);

        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response = ResponseFactory<byte[]>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);

        //        return Ok(response);
        //    }
        //}
    }
}