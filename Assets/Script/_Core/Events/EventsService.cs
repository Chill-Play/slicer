using System;
using System.Collections.Generic;


namespace GameFramework.Core
{

    [AutoInitializeService]
    public class EventsService : IService
    {
        #region Properties

        public bool Enabled { get; set; }

        Dictionary<int, Dictionary<Type, EventContainer>> containers = new Dictionary<int, Dictionary<Type, EventContainer>>();
        Dictionary<int, EventContainerSimple> eventContainerSimple = new Dictionary<int, EventContainerSimple>();

        #endregion



        #region Public Methods

        public void SendEventGlobal<T>(int eventId, T _data)
        {
            EventContainer<T> container = GetContainer<T>(eventId);
            container.SendEvent(_data);
        }


        public void SendEventLocal<T>(IObject target, int eventId, T _data)
        {
            EventContainer<T> container = GetContainer<T>(eventId);
            container.SendEvent(target, _data);
        }


        public void AddEventReceiver<T>(IEventReceiver<T> _receiver)
        {
            EventContainer<T> container = GetContainer<T>(_receiver.EventId);
            container.AddEventReceiver(_receiver);
        }


        public void RemoveEventReceiver<T>(IEventReceiver<T> _receiver)
        {
            EventContainer<T> container = GetContainer<T>(_receiver.EventId);
            container.RemoveEventReceiver(_receiver);
        }


        public T GetCachedData<T>(EventType eventType)
        {
            return DataVault.Instance.Pull<T>(eventType.DataId);
        }


        public void SendEventGlobal(int eventId)
        {
            EventContainerSimple container = GetContainerSimple(eventId);
            container.SendEvent();
        }


        public void SendEventLocal(IObject target, int eventId)
        {
            EventContainerSimple container = GetContainerSimple(eventId);
            container.SendEvent(target);
        }


        public void AddEventReceive(IEventReceiverSimple _receiver)
        {
            EventContainerSimple container = GetContainerSimple(_receiver.EventId);
            container.AddEventReceiver(_receiver);
        }


        public void RemoveEventReceiver(IEventReceiverSimple _receiver)
        {
            EventContainerSimple container = GetContainerSimple(_receiver.EventId);
            container.RemoveEventReceiver(_receiver);
        }


        public EventContainer<T> GetEventContainer<T>(int eventId)
        {
            return GetContainer<T>(eventId);
        }

        #endregion



        #region Private Methods

        EventContainerSimple GetContainerSimple(int _eventId)
        {
            if(eventContainerSimple == null)
            {
                eventContainerSimple = new Dictionary<int, EventContainerSimple>();
            }

            if (!eventContainerSimple.ContainsKey(_eventId))
            {
                eventContainerSimple.Add(_eventId, new EventContainerSimple());
            }

            return eventContainerSimple[_eventId];
        }


        EventContainer<T> GetContainer<T>(int _eventId)
        {
            if(containers == null)
            {
                containers = new Dictionary<int, Dictionary<Type, EventContainer>>();
            }
            if (!containers.ContainsKey(_eventId))
            {
                containers.Add(_eventId, new Dictionary<Type, EventContainer>());
            }

            Dictionary<Type, EventContainer> eventTypeContainers = containers[_eventId];

            Type type = typeof(T);

            if (!eventTypeContainers.ContainsKey(type))
            {
                EventContainer container = new EventContainer<T>();
                eventTypeContainers.Add(type, container);
            }

            return (EventContainer<T>)eventTypeContainers[type];
        }

        #endregion
    }
}
