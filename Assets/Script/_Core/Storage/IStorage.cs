using UnityEngine;
using System.Collections;

namespace GameFramework.Core
{
    public interface IStorageService
    {
        void AddValue<T>(T data, int id);
        T LoadValue<T>(int id);

        void Save();
        void Load();

        //void SaveAsync();
        //void LoadAsync();
    }
}