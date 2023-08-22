using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToolsLibrary.Models
{
    public class User : BaseModel
    {
        public ICollection<Advertisement> Advertisements { get; set; }

        [Required(ErrorMessage = $"{nameof(Email)} is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = $"{nameof(FullName)} is required.")]
        public string FullName { get; set; }

        [ForeignKey("LoginId")]
        public Login Login { get; set; }

        public int? LoginId { get; set; }

        [Required(ErrorMessage = $"{nameof(Phone)} is required.")]
        public string Phone { get; set; }
    }
}