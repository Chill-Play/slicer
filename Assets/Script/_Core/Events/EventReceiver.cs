using System;
using UnityEngine;


namespace GameFramework.Core
{
    public class EventReceiver<T> : IEventReceiver<T>
    {
        #region Variables

        [SerializeField] EventType eventType;

        Action<T> callback;

        #endregion



        #region Properties

        public EventType EventType { get => eventType; set { eventType = value; } }

        public T CachedData 
        {
            get
            {
               return IoCContainer.Get<EventsService>().GetCachedData<T>(eventType);
            }
        }

        public int EventId
        {
            get
            {
                return EventType.Id;
            }
        }

        public IObject Owner { get; private set; }

        #endregion



        #region Public Methods

        public void Subscribe(IObject owner, Action<T> _callback)
        {
            callback = _callback;
            this.Owner = owner;
            IoCContainer.Get<EventsService>().AddEventReceiver(this);
        }


        public void Unsubscribe()
        {
            callback = null;

            IoCContainer.Get<EventsService>().RemoveEventReceiver(this);
        }


        public void FireEvent(T _data)
        {
            callback?.Invoke(_data);
        }

        #endregion
    }


    public class SimpleEventReceiver : EventReceiver<object>
    {
    }
}