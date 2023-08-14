namespace ToolsLibrary.Models
{
    public class Advertisement : BaseModel
    {
        public byte[] Content { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}