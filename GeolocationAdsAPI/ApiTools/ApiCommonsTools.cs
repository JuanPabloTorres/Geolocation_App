using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAdsAPI.ApiTools
{
    public static class ApiCommonsTools
    {
        public static async Task<ResponseTool<IFormCollection>> GetFormDataFromHttpRequest(HttpRequest request)
        {
            try
            {
                var formCollection = await request.ReadFormAsync();

                return ResponseFactory<IFormCollection>.BuildSuccess("Request Form Data Found", formCollection, ToolsLibrary.Tools.Type.DataFound);
            }
            catch (InvalidDataException ex)
            {
                return ResponseFactory<IFormCollection>.BuildFail(ex.Message, null, ToolsLibrary.Tools.Type.Exception);
            }
        }

        public static byte[] Combine(List<byte[]> blocks)
        {
            // Paso 1: Calcular el tamaño total
            int totalSize = blocks.Sum(b => b.Length);

            // Paso 2: Crear el array de bytes final
            byte[] result = new byte[totalSize];

            // Paso 3: Copiar cada bloque en el array final
            int offset = 0;
            foreach (byte[] block in blocks)
            {
                Array.Copy(block, 0, result, offset, block.Length);
                offset += block.Length;
            }

            return result;
        }

        public static ContentVisualType DetermineContentType(string mimeType)
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
    }
}