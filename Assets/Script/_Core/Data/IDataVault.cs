using UnityEngine;
using System.Collections;


namespace GameFramework.Core
{
    public interface IDataVault
    {
        void Push<T>(T data, int id, bool serialize = false);


        void Push<T>(T data, IDataId dataId, bool serialize = false);


        T Pull<T>(int id, T defaultValue = default(T), bool serialized = false);


        T Pull<T>(IDataId dataId, T defaultValue = default(T), bool serialized = false);
    }
}