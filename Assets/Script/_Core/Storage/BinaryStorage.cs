using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace GameFramework.Core
{
    [AutoInitializeService]
    public class BinaryStorageService : IService
    {
        #region Variables

        const string FILE_NAME = "Storage.sav";
        Dictionary<System.Type, Dictionary<int, object>> storage = new Dictionary<System.Type, Dictionary<int, object>>();

        bool loaded = false;

        #endregion


        #region Properties

        Dictionary<System.Type, Dictionary<int, object>> Storage
        {
            get
            {
                if (storage == null)
                {
                    storage = new Dictionary<System.Type, Dictionary<int, object>>();
                }
                return storage;
            }
        }

        #endregion


        #region IService

        public bool Enabled { get; set; }

        #endregion




        #region IStorage

        public void AddValue<T>(T data, int id)
        {
            System.Type type = typeof(T);
            if(!Storage.ContainsKey(type))
            {
                Storage.Add(type, new Dictionary<int, object>());
            }
            if (!Storage[type].ContainsKey(id))
            {
                Storage[type].Add(id, data);
            }
            else
            {
                Storage[type][id] = data;
            }
        }


        public T LoadValue<T>(int id)
        {
            if(!loaded)
            {
                Load();
            }
            System.Type type = typeof(T);
            if (!Storage.ContainsKey(type))
            {
                Storage.Add(type, new Dictionary<int, object>());
            }
            if (!Storage[type].ContainsKey(id))
            {
                Storage[type].Add(id, default(T));
            }

            return (T)Storage[type][id];
        }

            
        public void Load()
        {
            if (File.Exists(GetStorageFilePath()))
            {
                FileStream fs = new FileStream(GetStorageFilePath(), FileMode.Open);
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    storage = (Dictionary<System.Type, Dictionary<int, object>>)formatter.Deserialize(fs);
                }
                catch (SerializationException e)
                {
                    Debug.Log("Failed to deserialize storage file. Reason: " + e.Message);
                    throw;
                }
                finally
                {
                    fs.Close();
                    loaded = true;
                }
            }
        }


        public void Save()
        {

            FileStream fs = new FileStream(GetStorageFilePath(), FileMode.Create);

            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, Storage);
            }
            catch (SerializationException e)
            {
                Debug.Log("Failed to serialize storage file. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        #endregion



        #region Private methods

        string GetStorageFilePath()
        {
            return Path.Combine(Application.persistentDataPath, FILE_NAME);
        }

        #endregion
    }
}