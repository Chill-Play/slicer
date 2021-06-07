using UnityEngine;
using System.Collections;


namespace GameFramework.Core
{
    [System.Serializable]
    public class DataSupplier<T>
    {
        #region Properties

        public DataId Id { get; set; }

        #endregion



        #region Public methods

        public T GetData()
        {
            return DataVault.Instance.Pull<T>(Id);
        }


        public void SetData(T data)
        {
            DataVault.Instance.Push(data, Id);
        }

        #endregion

    }
}