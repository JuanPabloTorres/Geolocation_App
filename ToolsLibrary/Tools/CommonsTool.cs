using Microsoft.Extensions.Configuration;
using System.IO.Compression;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using ToolsLibrary.Models;

namespace ToolsLibrary.Tools
{
    public static class CommonsTool
    {
        // Compress a byte array using GZip compression
        public static byte[] Compress(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                {
                    gzipStream.Write(data, 0, data.Length);
                }
                return memoryStream.ToArray();
            }
        }

        // Decompress a GZip-compressed byte array
        public static byte[] Decompress(byte[] compressedData)
        {
            using (MemoryStream memoryStream = new MemoryStream(compressedData))
            using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            using (MemoryStream decompressedStream = new MemoryStream())
            {
                gzipStream.CopyTo(decompressedStream);

                return decompressedStream.ToArray();
            }
        }

        public static async Task DisplayAlert(string title, string message)
        {
            await Shell.Current.DisplayAlert(title, message, "OK");
        }

        public static byte[] ExtractFirstFrame(byte[] videoBytes)
        {
            // Find the start of the video data in the byte array
            int startIndex = FindVideoStartIndex(videoBytes);

            if (startIndex == -1)
            {
                throw new InvalidOperationException("Video data not found in the byte array.");
            }

            // Extract the video data starting from the identified index
            byte[] videoData = new byte[videoBytes.Length - startIndex];
            Array.Copy(videoBytes, startIndex, videoData, 0, videoData.Length);

            return videoData;
        }

        public static int FindVideoStartIndex(byte[] data)
        {
            // Search for the video data start marker in the byte array
            byte[] startMarker = { 0x00, 0x00, 0x00, 0x01 };
            for (int i = 0; i < data.Length - startMarker.Length; i++)
            {
                bool foundMarker = true;
                for (int j = 0; j < startMarker.Length; j++)
                {
                    if (data[i + j] != startMarker[j])
                    {
                        foundMarker = false;
                        break;
                    }
                }
                if (foundMarker)
                {
                    return i;
                }
            }
            return -1; // Start marker not found
        }

        public static string GenerateRandomCode(int length)
        {
            string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            Random random = new Random();

            char[] code = new char[length];

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(characters.Length);

                code[i] = characters[index];
            }

            return new string(code);
        }

        public static ContentVisualType GetContentType(string fileName)
        {
            switch (Path.GetExtension(fileName))
            {
                case ".jpg":
                case ".png":
                case ".gif":
                case ".jpeg":
                    return ContentVisualType.Image;

                case ".mp4":
                    return ContentVisualType.Video;

                default:
                    // Handle other file types or return a default value if necessary.
                    return ContentVisualType.Unknown;
            }
        }

        public static async Task<byte[]> GetFileBytesAsync(FileResult fileResult)
        {


            if (fileResult is null)
            {
                return null;
            }

            using var stream = await fileResult.OpenReadAsync();

            using var ms = new MemoryStream();

            await stream.CopyToAsync(ms);

            return ms.ToArray();
        }

        public static string HashPassword(string password)
        {
            // Generate a random salt
            string salt = BCrypt.Net.BCrypt.GenerateSalt(12); // You can adjust the cost factor as needed

            // Hash the password with the salt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            return hashedPassword;
        }

        public static string HtmlEmailRecoveryDesign(string code)
        {
            var bodyBuilder = new StringBuilder();

            bodyBuilder.AppendLine("<!DOCTYPE html>");

            bodyBuilder.AppendLine("<html lang=\"en\">");

            bodyBuilder.AppendLine("<head>");

            bodyBuilder.AppendLine("    <meta charset=\"UTF-8\">");

            bodyBuilder.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");

            bodyBuilder.AppendLine("    <title> Password Recovery</title>");

            bodyBuilder.AppendLine("    <style>");

            // Include the CSS styles from the previous response here
            bodyBuilder.AppendLine("    </style>");

            bodyBuilder.AppendLine("</head>");

            bodyBuilder.AppendLine("<body>");

            bodyBuilder.AppendLine("    <div class=\"container\">");

            bodyBuilder.AppendLine("        <div class=\"header\">");

            bodyBuilder.AppendLine("            <h1> Password Recovery</h1>");

            bodyBuilder.AppendLine("        </div>");

            bodyBuilder.AppendLine("        <div class=\"content\">");

            bodyBuilder.AppendLine($"<div><b>{code}</b></div>"); // This is where you insert the dynamic content

            bodyBuilder.AppendLine("        </div>");

            bodyBuilder.AppendLine("        <div class=\"footer\">");

            bodyBuilder.AppendLine("            <p>If you need further assistance, please contact our support team.</p>");

            bodyBuilder.AppendLine("        </div>");

            bodyBuilder.AppendLine("    </div>");

            bodyBuilder.AppendLine("</body>");

            bodyBuilder.AppendLine("</html>");

            return bodyBuilder.ToString();
        }

        public static async Task<byte[]> ImageSourceToByteArrayAsync()
        {
            var assem = Assembly.GetExecutingAssembly();

            using var stream = assem.GetManifestResourceStream("GeolocationAds.Resources.Images.mediacontent.png");

            byte[] bytesAvailable = new byte[stream.Length];

            await stream.ReadAsync(bytesAvailable, 0, bytesAvailable.Length);

            return bytesAvailable;
        }

        public static async Task<byte[]> ImageSourceToByteArrayAsync(ImageSource imageSource)
        {
            if (imageSource is FileImageSource fileImageSource)
            {
                var assem = Assembly.GetExecutingAssembly();

                string resourceName = fileImageSource.File;

                using Stream stream = assem.GetManifestResourceStream(resourceName);

                if (stream != null)
                {
                    using MemoryStream memoryStream = new MemoryStream();

                    await stream.CopyToAsync(memoryStream);

                    return memoryStream.ToArray();
                }
                else
                {
                    throw new FileNotFoundException($"Resource '{resourceName}' not found in assembly.");
                }
            }
            else
            {
                throw new ArgumentException("Unsupported ImageSource type.");
            }
        }

        public static async Task<string> SaveByteArrayToTempFile(byte[] byteArray)
        {
            try
            {
                string tempFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                // Clean up existing temporary files in the folder
                CleanUpTempFiles(tempFolderPath);

                string tempFileName = Guid.NewGuid().ToString().Take(4).ToString(); // Use a unique filename

                string tempFilePath = Path.Combine(tempFolderPath, tempFileName);

                await File.WriteAllBytesAsync(tempFilePath, byteArray);

                return tempFilePath;
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }

            return string.Empty;
        }

        public static async Task<string> SaveByteArrayToTempFile2(byte[] byteArray)
        {
            try
            {
                // Generate a unique file name, e.g., using a GUID or a timestamp
                var uniqueFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".tmp");

                // Write the byte array to the newly created file
                await File.WriteAllBytesAsync(uniqueFileName, byteArray);

                return uniqueFileName;
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);

                return string.Empty;
            }
        }

        public static async Task<string> SaveByteArrayToFile(byte[] byteArray, string filePath = null)
        {
            try
            {
                // If filePath is null, generate a new unique file name
                if (string.IsNullOrEmpty(filePath))
                {
                    filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".mp4");  // Using .mp4 for clarity
                }

                // Append the byte array to the file using FileMode.Append
                using (var fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None))
                {
                    await fileStream.WriteAsync(byteArray, 0, byteArray.Length);
                }

                return filePath;
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
                return string.Empty;
            }
        }

        //public static async Task<string> SaveByteArrayToPartialFile3(byte[] byteArray, string filePath, long startBlock=0, long endBlock=ConstantsTools.SegmentSize)
        //{
        //    try
        //    {
        //        // If no file path is provided, generate a unique file name
        //        if (string.IsNullOrEmpty(filePath))
        //        {
        //            filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".mp4");  // Using .mp4 as the extension for clarity
        //        }

        //        // Append the byte array to the file
        //        using (var stream = new FileStream(filePath, FileMode.Append, FileAccess.Write))
        //        {
        //            await stream.WriteAsync(byteArray, 0, byteArray.Length);
        //        }

        //        return filePath;
        //    }
        //    catch (Exception ex)
        //    {
        //        await CommonsTool.DisplayAlert("Error", ex.Message);

        //        return string.Empty;
        //    }
        //}

        //public static async Task<string> SaveByteArrayToPartialFile3(byte[] byteArray, string filePath, long startBlock = 0, long endBlock = ConstantsTools.SegmentSize)
        //{
        //    try
        //    {
        //        // If no file path is provided, generate a unique file name
        //        if (string.IsNullOrEmpty(filePath))
        //        {
        //            filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".mp4");  // Using .mp4 as the extension for clarity
        //        }

        //        // Ensure the file exists, if not create it
        //        if (!File.Exists(filePath))
        //        {
        //            using (var createStream = File.Create(filePath))
        //            {
        //                // Optionally initialize file to expected size to optimize disk allocation
        //                if (endBlock > 0)
        //                {
        //                    createStream.SetLength(endBlock);
        //                }
        //            }
        //        }

        //        // Open the file with the ability to seek, then write the bytes at the specific start block position
        //        using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Write, FileShare.None))
        //        {
        //            stream.Seek(startBlock, SeekOrigin.Begin);

        //            await stream.WriteAsync(byteArray, 0, byteArray.Length);
        //        }

        //        return filePath;
        //    }
        //    catch (Exception ex)
        //    {
        //        await CommonsTool.DisplayAlert("Error", ex.Message);

        //        return string.Empty;
        //    }
        //}

        public static async Task<string> SaveByteArrayToPartialFile3(byte[] byteArray, string filePath, long startBlock = 0, long endBlock = ConstantsTools.SegmentSize)
        {
            int maxRetries = 5;

            int attempt = 0;

            while (attempt < maxRetries)
            {
                try
                {
                    if (string.IsNullOrEmpty(filePath))
                    {
                        filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".mp4");
                    }

                    if (!File.Exists(filePath))
                    {
                        using (var createStream = File.Create(filePath))
                        {
                            if (endBlock > 0)
                            {
                                createStream.SetLength(endBlock);
                            }
                        }
                    }

                    using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Write, FileShare.Read))
                    {
                        stream.Seek(startBlock, SeekOrigin.Begin);

                        await stream.WriteAsync(byteArray, 0, byteArray.Length);
                    }

                    return filePath;
                }
                catch (IOException ex)
                {
                    attempt++;

                    if (attempt >= maxRetries)
                    {
                        await CommonsTool.DisplayAlert("Error", "Failed to write to file after several attempts: " + ex.Message);

                        return string.Empty;
                    }

                    // Wait for a bit before retrying
                    await Task.Delay(200 * attempt);
                }
            }

            return string.Empty; // This should be unreachable
        }

        public static async Task SendEmailAsync(EmailRequest emailRequest, IConfiguration configuration)
        {
            var _port = int.Parse(configuration["Smtp:Port"]);

            var smtpClient = new SmtpClient(configuration["Smtp:Server"])
            {
                Port = _port,
                Credentials = new NetworkCredential(configuration["Smtp:Username"], configuration["Smtp:Password"]),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(configuration["Smtp:Username"], "Geolocation App"),

                Subject = emailRequest.Subject,

                Body = emailRequest.Body,

                IsBodyHtml = true,
            };

            mailMessage.To.Add(emailRequest.To);

            await smtpClient.SendMailAsync(mailMessage);
        }

        private static async void CleanUpTempFiles(string folderPath)
        {
            try
            {
                foreach (string filePath in Directory.GetFiles(folderPath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during cleanup
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        public static long GetFileSize(byte[] fileBytes)
        {
            return fileBytes.Length;
        }
    }
}