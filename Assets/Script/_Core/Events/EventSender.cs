using System.Runtime.Serialization;
using UnityEngine;


namespace GameFramework.Core
{
    [System.Serializable]
    public class testEv
    {

    }

    [System.Serializable]
    public class EventSender<T> : testEv
    {
        #region Variables

        [SerializeField] EventType eventType;
        EventContainer<T> cachedContainer;

        #endregion


        #region Properties

        public EventType EventType { get => eventType; set { eventType = value; } }

        #endregion


        #region Public Methods

        public void SendEventGlobal(T _data)
        {
            if(cachedContainer == null)
            {
                CacheEventContainer();
            }
            cachedContainer.SendEvent(_data);
        }


        public void SendEventLocal(IObject target, T _data)
        {
            if (cachedContainer == null)
            {
                CacheEventContainer();
            }
            cachedContainer.SendEvent(target, _data);
        }

        #endregion



        #region Private methods

        [OnDeserializedAttribute()]
        void OnDeserialize(StreamingContext context)
        {
          /* if(Application.isPlaying)
            {
                CacheEventContainer();
            }*/
        }

        void CacheEventContainer()
        {
            cachedContainer = IoCContainer.Get<EventsService>().GetEventContainer<T>(eventType.Id);
        }

        #endregion
    }


    public class SimpleEventSender : EventSender<object>
    {
    }
}