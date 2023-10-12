namespace ToolsLibrary.Models
{
    public class Pagination<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int Index { get; set; }
        public int DataSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / DataSize);

        public Pagination()
        {
            Items = new List<T>();
        }

        public Pagination(List<T> items, int totalItems, int pageNumber, int pageSize)
        {
            Items = items;

            TotalItems = totalItems;

            Index = pageNumber;

            DataSize = pageSize;
        }
    }
}