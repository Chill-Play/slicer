using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace GameFramework.Core
{
    public abstract class DataContainer { }

    public class DataContainer<T> : DataContainer
    {
        #region Variables

        Dictionary<int, T> dataCollection = new Dictionary<int, T>();

        #endregion



        #region Public methods

        public void Push(T data, int id, bool serialize = false)
        {
            if (!dataCollection.ContainsKey(id))
            {
                dataCollection.Add(id, data);
            }
            else
            {
                dataCollection[id] = data;
            }
            if(serialize)
            {
                IoCContainer.Get<IStorageService>().AddValue<T>(data, id);
            }
        }


        public T Pull(int id, T defaultValue,  bool serialized = false)
        {
            T data = defaultValue;
            if (serialized)
            {
                data = IoCContainer.Get<IStorageService>().LoadValue<T>(id);
            }

            if (!dataCollection.ContainsKey(id))
            {
                dataCollection.Add(id, data);
            }
            return dataCollection[id];
        }

        #endregion
    }
}