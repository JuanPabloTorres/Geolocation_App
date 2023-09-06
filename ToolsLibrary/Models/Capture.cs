namespace ToolsLibrary.Models
{
    public class Capture : BaseModel
    {
        public int UserId { get; set; }

        public ICollection<Advertisement> Advertisements { get; set; }
    }
}
