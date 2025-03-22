namespace GeolocationAdsAPI.ApiTools
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using ToolsLibrary.Models;
    using ToolsLibrary.Tools;

    public static class FileManager
    {
        private const long MaxFileSize = ConstantsTools.MaxFileSize;

        /// <summary>
        /// Valida si un archivo es válido según el tamaño permitido.
        /// </summary>
        public static bool IsValidFileSize(IFormFile file)
        {
            return file.Length <= MaxFileSize;
        }

        /// <summary>
        /// Convierte un archivo a un arreglo de bytes.
        /// </summary>
        public static async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            using var memoryStream = new MemoryStream();

            await file.CopyToAsync(memoryStream);

            return memoryStream.ToArray();
        }

        /// <summary>
        /// Obtiene los metadatos de un archivo desde sus encabezados.
        /// </summary>
        public static Dictionary<string, string> ExtractFileMetadata(IFormFile file)
        {
            return new Dictionary<string, string>
        {
            { "FilePath", file.Headers["FilePath"].FirstOrDefault() ?? string.Empty },
            { "ID", file.Headers["ID"].FirstOrDefault() ?? "0" },
            { "AdId", file.Headers["AdId"].FirstOrDefault() ?? "0" }
        };
        }

        /// <summary>
        /// Crea un objeto ContentType a partir de un archivo.
        /// </summary>
        public static async Task<ContentType> CreateContentFromFileAsync(IFormFile file)
        {
            if (!IsValidFileSize(file))
            {
                throw new InvalidOperationException($"File size exceeds the limit: {file.FileName}");
            }

            var metadata = ExtractFileMetadata(file);

            return new ContentType
            {
                Content = await ConvertFileToByteArrayAsync(file),
                FilePath = metadata["FilePath"],
                FileSize = file.Length,
                ContentName = file.FileName,
                Type = ApiCommonsTools.DetermineContentType(file.ContentType),
                CreateDate = DateTime.UtcNow,
                ID = int.Parse(metadata["ID"]),
                AdvertisingId = int.Parse(metadata["AdId"])
            };
        }

        /// <summary>
        /// Procesa una colección de archivos y devuelve una lista de ContentType.
        /// </summary>
        public static async Task<List<ContentType>> ProcessUploadedFilesAsync(IFormCollection formCollection)
        {
            var contents = new List<ContentType>();

            foreach (var file in formCollection.Files)
            {
                var content = await CreateContentFromFileAsync(file);

                contents.Add(content);
            }

            return contents;
        }
    }

}
