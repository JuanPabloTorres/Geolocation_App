using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ToolsLibrary.Enums;

namespace ToolsLibrary.Models
{
    public class Login : BaseModel
    {
        public Login()
        {
        }

        public string? GoogleId { get; set; }
        
        public string? FacebookId { get; set; }

        public string HashPassword { get; set; }

        [Required(ErrorMessage = $"{nameof(Password)} is required.")]
        public string? Password { get; set; }

        public Providers Provider { get; set; }

        [ForeignKey("UserId")]
        [JsonIgnore]
        public User User { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessage = $"{nameof(Username)} is required.")]
        public string? Username { get; set; }

    }
}