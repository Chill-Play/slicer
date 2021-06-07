using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IObjectPool<T>
{
    void ReturnObject(T obj);
    T GetObject();
}
