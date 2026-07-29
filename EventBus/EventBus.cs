using Godot;
using System.Collections.Generic;
using System;
using System.Linq;

    public class EventBus<TBase> : IEventBus<TBase> where TBase : class
    {
        private readonly Dictionary<Type, List<Delegate>> handlers = new Dictionary<Type, List<Delegate>>();
        private readonly object threadLock = new();

        public void Subscribe<T>(Action<T> handler) where T : TBase
        {
            lock (threadLock)
            {
                var type = typeof(T);
                if (!handlers.ContainsKey(type))
                    handlers[type] = new List<Delegate>();
                handlers[type].Add(handler);
            }
        }
        public void Unsubscribe<T>(Action<T> handler) where T : TBase
        {
            lock (threadLock)
                if (handlers.TryGetValue(typeof(T), out var list))
                    list.Remove(handler);
        }

        public void Publish<T>(T eventData) where T : TBase
        {
            List<Delegate> snapshot;
            lock (threadLock)
            {
                if (!handlers.TryGetValue(typeof(T), out var list)) return;
                snapshot = list.ToList();
            }
            foreach (var handler in snapshot)
                ((Action<T>)handler)(eventData);//<--If this line is confusing
                                                //This is the long hand version of what is going on.
                                                //Action<T> typeHandler = (Action<T>)handler;
                                                //typeHandler.Invoke(eventData);
        }
    }
