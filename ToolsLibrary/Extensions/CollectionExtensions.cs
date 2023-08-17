using System.Collections.ObjectModel;

namespace ToolsLibrary.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                list.Add(item);
            }
        }

        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        // IEnumerable doesn't have a built-in Add method, so this extension returns a new collection
        public static IEnumerable<T> AddRange<T>(this IEnumerable<T> collection, IEnumerable<T> items)
        {
            return collection.Concat(items);
        }
    }
}