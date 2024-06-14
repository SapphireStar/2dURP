using System;
using System.Collections.Generic;

namespace MyPackage
{
    public class EventClass<T>
    {
        public Action<T> Actions;
        public T e;
        public void Subscribe(Action<T> action)
        {
            Actions += action;
        }
        public void Unsubscribe(Action<T> action)
        {
            Actions -= action;
        }
        public void Invoke()
        {
            Actions?.Invoke(e);
        }
    }
    public class EventSystem : Singleton<EventSystem>
    {
        Dictionary<Type, object> dictionary;
        public EventSystem()
        {
            dictionary = new Dictionary<Type, object>();
        }

        bool Contains(Type type) => dictionary.ContainsKey(type);

        public void Subscribe<T>(Type type, Action<T> action) where T : IEventHandler
        {
            if (!Contains(type))
            {
                dictionary.Add(type, new EventClass<T>());
            }
            EventClass<T> ec = dictionary[type] as EventClass<T>;
            ec.Subscribe(action);
        }


        public void Unsubscribe<T>(Type type, Action<T> action)
        {
            if (!Contains(type))
            {
                return;
            }
            EventClass<T> ec = dictionary[type] as EventClass<T>;
            ec.Unsubscribe(action);
        }



        public void SendEvent<T>(Type type, T handler) where T : IEventHandler
        {
            if (Contains(type))
            {
                EventClass<T> ec = dictionary[type] as EventClass<T>;
                ec.e = handler;
                ec.Invoke();
            }
        }
    }

}
