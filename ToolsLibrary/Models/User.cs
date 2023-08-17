using System.ComponentModel.DataAnnotations.Schema;

namespace ToolsLibrary.Models
{
    public class User : BaseModel
    {
        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public int LoginId { get; set; }

        [ForeignKey("LoginId")]
        public LoginCredential Login { get; set; }
    }
}
