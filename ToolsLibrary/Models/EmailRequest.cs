using Microsoft.AspNetCore.Http;

namespace ToolsLibrary.Models
{
    public class EmailRequest
    {
        public List<IFormFile> Attachments { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string To { get; set; }
    }
}