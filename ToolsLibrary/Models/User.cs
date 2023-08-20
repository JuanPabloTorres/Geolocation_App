using System.ComponentModel.DataAnnotations.Schema;

namespace ToolsLibrary.Models
{
    public class User : BaseModel
    {
        public ICollection<Advertisement> Advertisements { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        [ForeignKey("LoginId")]
        public LoginCredential Login { get; set; }

        public int LoginId { get; set; }

        public string Phone { get; set; }
    }
}