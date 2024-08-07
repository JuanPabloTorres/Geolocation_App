using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ToolsLibrary.Models
{
    public class Login : BaseModel
    {
        [Required(ErrorMessage = $"{nameof(Username)} is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = $"{nameof(Password)} is required.")]
        public string Password { get; set; }

        public string HashPassword { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        [JsonIgnore]
        public User User { get; set; }

        public Login()
        {
        }
    }
}