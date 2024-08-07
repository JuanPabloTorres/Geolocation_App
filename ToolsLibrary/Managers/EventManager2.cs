namespace ToolsLibrary.Managers
{
    public class EventManager2
    {
        private static EventManager2 _instance;

        private static readonly object _lock = new object();

        private Dictionary<Type, Dictionary<string, List<Delegate>>> subscribers = new Dictionary<Type, Dictionary<string, List<Delegate>>>();

        public static EventManager2 Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new EventManager2();
                        }
                    }
                }
                return _instance;
            }
        }

        public void Publish<TEventType>(TEventType eventArgs, string context)
        {
            var eventType = typeof(TEventType);

            Dictionary<string, List<Delegate>> contextSubscribers;

            lock (_lock)
            {
                if (subscribers.TryGetValue(eventType, out contextSubscribers))
                {
                    if (contextSubscribers.TryGetValue(context, out List<Delegate> toInvoke))
                    {
                        foreach (var subscriber in toInvoke)
                        {
                            ((Action<TEventType>)subscriber)(eventArgs);
                        }
                    }
                }
            }
        }

        public void Subscribe<TEventType>(Action<TEventType> subscriber, object caller)
        {
            string context = caller.GetType().Name;

            var eventType = typeof(TEventType);

            lock (_lock)
            {
                if (!subscribers.ContainsKey(eventType))
                {
                    subscribers[eventType] = new Dictionary<string, List<Delegate>>();
                }

                if (!subscribers[eventType].ContainsKey(context))
                {
                    subscribers[eventType][context] = new List<Delegate>();
                }

                // Check if the subscriber is already registered
                var contextSubscribers = subscribers[eventType][context];

                // If there are no subscribers yet or if there is, replace it with the new subscriber
                if (contextSubscribers.Count == 0)
                {
                    contextSubscribers.Add(subscriber);
                }
                else
                {
                    // Replace the existing subscriber with the new one
                    contextSubscribers[0] = subscriber;
                }
            }
        }
    }
}