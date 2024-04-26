using Azure;
using GeolocationAdsAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
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
        private readonly IAdvertisementRepository advertisementRepository;

        private readonly IContentTypeRepository contentTypeRepository;

        public AdvertisementController(IAdvertisementRepository advertisementRepository, IContentTypeRepository contentTypeRepository)
        {
            this.advertisementRepository = advertisementRepository;

            this.contentTypeRepository = contentTypeRepository;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(Advertisement advertisement)
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

        // You can name this action method as per your routing preferences.
        [HttpPost("Add2")]
        public async Task<IActionResult> AddAdvertisement()
        {
            if (!Request.HasFormContentType)
            {
                return BadRequest("Unsupported media type");
            }

            var formCollection = await Request.ReadFormAsync();

            var advertisementJson = formCollection["advertisementMetadata"];

            var advertisement = JsonConvert.DeserializeObject<Advertisement>(advertisementJson);

            //advertisement?.Contents.Clear();

            // Assuming ContentType has IFormFile or similar for Content
            //foreach (var file in formCollection.Files)
            //{
            //    var content = new ContentType
            //    {
            //        ContentName = file.FileName,
            //        Type = GetContentVisualType(file.ContentType),
            //        Content = await ConvertToByteArray(file)
            //        // Assign other properties as needed
            //    };

            //    advertisement?.Contents.Add(content);
            //}

            // TODO: Handle GeolocationAds and Settings if they are part of the form

            try
            {
                var response = await this.advertisementRepository.CreateAsync(advertisement);

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = ResponseFactory<Advertisement>.BuildFail(ex.Message, null);

                return StatusCode(StatusCodes.Status500InternalServerError, response);
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

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> StreamingVideo(int id)
        {
            try
            {
                var responseResult = await contentTypeRepository.GetContentById(id); // Método para obtener los bytes del video de la base de datos

                if (responseResult.Data.IsObjectNull())
                {
                    return Ok(ResponseFactory<string>.BuildFail("Data Not Found", null, ToolsLibrary.Tools.Type.NotFound));

                    return;
                }

                byte[] videoBytes = responseResult.Data.Content;

                var videoFile = SaveBytesToFile(videoBytes);

                var hlsOutput = ConvertVideoToHLS3(videoFile);

                // Cleanup: Elimina el archivo de video temporal
                System.IO.File.Delete(videoFile);

                //var _response = ResponseFactory<string>.BuildSusccess("Streaming Path", $"{Request.Scheme}://{Request.Host}/{hlsOutput}");

                var _response = ResponseFactory<string>.BuildSusccess("Streaming Path", $"{Request.Scheme}://192.168.0.11:5160{hlsOutput.Replace("wwwroot", "")}");

                //return Ok($"{Request.Scheme}://{Request.Host}/{hlsOutput}");

                return Ok(_response);
            }
            catch (Exception ex)
            {
                return Ok(ResponseFactory<string>.BuildFail(ex.Message, string.Empty, ToolsLibrary.Tools.Type.Exception));
            }
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

        private string ConvertVideoToHLS(string videoFilePath)
        {
            var outputDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            Directory.CreateDirectory(outputDirectory);

            var startInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-i {videoFilePath} -codec: copy -start_number 0 -hls_time 10 -hls_list_size 0 -f hls {Path.Combine(outputDirectory, "output.m3u8")}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(startInfo))
            {
                process.WaitForExit();
            }

            return outputDirectory;
        }

        private string ConvertVideoToHLS2(string videoFilePath)
        {
            var outputDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(outputDirectory);

            //var startInfo = new ProcessStartInfo
            //{
            //    FileName = "C:\ffmpeg\bin", // Asegúrate de que "ffmpeg" se puede llamar directamente
            //    Arguments = $"-i \"{videoFilePath}\" -codec: copy -start_number 0 -hls_time 10 -hls_list_size 0 -f hls \"{Path.Combine(outputDirectory, "output.m3u8")}\"",
            //    RedirectStandardOutput = true,
            //    RedirectStandardError = true,
            //    UseShellExecute = false,
            //    CreateNoWindow = true
            //};

            var startInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = "-version",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(startInfo))
            {
                process.WaitForExit();
                string output = process.StandardOutput.ReadToEnd();
                Console.WriteLine(output);
            }

            Debug.WriteLine("PATH is: " + Environment.GetEnvironmentVariable("PATH"));

            using (var process = Process.Start(startInfo))
            {
                process.WaitForExit();

                // Asegúrate de capturar y revisar la salida de error
                string stderr = process.StandardError.ReadToEnd();
                if (!string.IsNullOrEmpty(stderr))
                {
                    // Maneja el error aquí
                    throw new Exception(stderr);
                }
            }

            return outputDirectory;
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