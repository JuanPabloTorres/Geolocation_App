using System.ComponentModel.DataAnnotations;

namespace ToolsLibrary.Models
{
    public class Login : BaseModel
    {
        [Required(ErrorMessage = $"{nameof(Username)} is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = $"{nameof(Password)} is required.")]
        public string Password { get; set; }

        public int UserId { get; set; }
    }
}