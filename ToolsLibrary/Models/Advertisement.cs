using System.ComponentModel.DataAnnotations;

namespace ToolsLibrary.Models
{
    public class Advertisement : BaseModel
    {
        [Required(ErrorMessage = $"{nameof(Content)} is required.")]
        public byte[] Content { get; set; }

        [Required(ErrorMessage = $"{nameof(Title)} is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = $"{nameof(Description)} is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = $"{nameof(ExpirationDate)} is required.")]
        public DateTime ExpirationDate { get; set; }

        public bool IsPosted { get; set; }

        public int UserId { get; set; }

        public Advertisement()
        {

        }

        public Advertisement(DateTime experationDate)
        {
            this.ExpirationDate = experationDate;
        }
    }
}