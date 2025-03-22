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
        public static void AddRange<T>(this List<T> collection, IEnumerable<T> items)
        {
            if (collection == null || items == null) return; // Evitar errores de nulos

            collection.AddRange(items);
        }


        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
        {
            List<T> batch = new List<T>(batchSize);

            foreach (T item in source)
            {
                batch.Add(item);

                if (batch.Count == batchSize)
                {
                    yield return batch;

                    batch = new List<T>(batchSize);
                }
            }

            if (batch.Any())
            {
                yield return batch;
            }
        }
    }
}