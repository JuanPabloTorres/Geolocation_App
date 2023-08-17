using System.ComponentModel.DataAnnotations.Schema;

namespace ToolsLibrary.Models
{
    public class LoginCredential : BaseModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
