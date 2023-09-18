namespace ToolsLibrary.Models
{
    public class DataLoader
    {
        public int DataSize { get; set; }

        public int Index { get; set; }

        public IEnumerable<int> CurrentIds { get; set; }
    }
}
