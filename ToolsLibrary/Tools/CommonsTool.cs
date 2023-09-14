using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using ToolsLibrary.Models;

namespace ToolsLibrary.Tools
{
    public static class CommonsTool
    {
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

        public static async Task<byte[]> GetFileBytesAsync(FileResult fileResult)
        {
            if (fileResult != null)
            {
                using (var stream = await fileResult.OpenReadAsync())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await stream.CopyToAsync(ms);

                        return ms.ToArray();
                    }
                }
            }
            return null;
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

        public static string HashPassword(string password)
        {
            // Generate a random salt
            string salt = BCrypt.Net.BCrypt.GenerateSalt(12); // You can adjust the cost factor as needed

            // Hash the password with the salt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            return hashedPassword;
        }
    }
}