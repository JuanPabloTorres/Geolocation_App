namespace ToolsLibrary.Models
{
    public class ForgotPassword : BaseModel
    {
        public bool IsValid { get; set; }

        public DateTime ExpTime { get; set; }

        public string Code { get; set; }

        public int UserId { get; set; }
    }
}