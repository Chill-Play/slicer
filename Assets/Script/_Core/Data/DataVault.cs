using System.Collections.Generic;
using UnityEngine;


namespace GameFramework.Core
{
    public class DataVault : Singleton<DataVault>, IDataVault
    {
        #region Variables

        Dictionary<System.Type, DataContainer> containers;

        #endregion



        #region Properties

        Dictionary<System.Type, DataContainer> Containers
        {
            get
            {
                if(containers == null)
                {
                    containers = new Dictionary<System.Type, DataContainer>();
                }
                return containers;
            }
        }

        #endregion



        #region IDataVault

        public void Push<T>(T data, int id, bool serialize = false)
        {
            UnityLogger.Instance.Log(LogLevel.Verbose, "Push data : " + data.ToString() + " to id : " + id);
            GetContainer<T>().Push(data, id, serialize);
        }


        public void Push<T>(T data, IDataId dataId, bool serialize = false)
        {
            UnityLogger.Instance.Log(LogLevel.Verbose, "Push data : " + data.ToString() + " to id : " + dataId.Id);
            GetContainer<T>().Push(data, dataId.Id, serialize);
        }


        public T Pull<T>(int id, T defaultValue = default(T), bool serialized = false)
        {
            return GetContainer<T>().Pull(id, defaultValue, serialized);
        }


        public T Pull<T>(IDataId dataId, T defaultValue = default(T), bool serialized = false)
        {
            return GetContainer<T>().Pull(dataId.Id, defaultValue, serialized);
        }

        #endregion



        #region Private Methods

        DataContainer<T> GetContainer<T>()
        {
            DataContainer container = null;
            System.Type dataType = typeof(T);
            if (!Containers.ContainsKey(dataType))
            {
                container = new DataContainer<T>();
                Containers.Add(dataType, container);
            }
            else
            {
                container = Containers[dataType];
            }


            return (DataContainer<T>)container;
        }

        #endregion

    }
}