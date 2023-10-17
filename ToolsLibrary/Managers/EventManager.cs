namespace ToolsLibrary.Managers
{
    public class EventManager
    {
        private static EventManager _instance;

        private Dictionary<Type, List<Delegate>> subscribers = new Dictionary<Type, List<Delegate>>();

        public static EventManager Instance
        {
            get { return _instance ?? (_instance = new EventManager()); }
        }

        public void Publish<TEventType>(TEventType eventArgs)
        {
            var eventType = typeof(TEventType);

            if (subscribers.ContainsKey(eventType))
            {
                foreach (var subscriber in subscribers[eventType])
                {
                    ((Action<TEventType>)subscriber)(eventArgs);
                }
            }
        }

        public void Subscribe<TEventType>(Action<TEventType> subscriber)
        {
            var eventType = typeof(TEventType);

            if (!subscribers.ContainsKey(eventType))
            {
                subscribers[eventType] = new List<Delegate>();
            }

            subscribers[eventType].Add(subscriber);
        }
    }
}