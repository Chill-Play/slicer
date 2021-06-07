using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameObjectsPool : MonoBehaviour, IObjectPool<Object>
{
    List<Object> insidePool = new List<Object>();
    List<Object> outsidePool = new List<Object>();
    Object prefab;


    public void Setup(Object prefab)
    {
        this.prefab = prefab;
        SpawnObject();
    }


    private void SpawnObject()
    {
        Object instance = Instantiate(prefab);
        insidePool.Add(instance);
    }


    public void ReturnObject(Object obj)
    {
        if (!outsidePool.Contains(obj))
        {
            return;
        }
        IPoolObject poolObject = null;
        if (obj is MonoBehaviour)
        {
            MonoBehaviour behaviour = obj as MonoBehaviour;
            poolObject = behaviour.GetComponent<IPoolObject>();
            behaviour.transform.SetParent(transform);
            behaviour.gameObject.SetActive(false);
        }
        else if (obj is GameObject)
        {
            GameObject go = obj as GameObject;
            poolObject = go.GetComponent<IPoolObject>();
            go.transform.parent = transform;
            go.gameObject.SetActive(false);
        }
        if (poolObject != null)
        {
            poolObject.OnReturnToPool();
        }

        outsidePool.Remove(obj);
        insidePool.Add(obj);
    }


    public Object GetObject()
    {
        if (insidePool.Count == 0)
        {
            SpawnObject();
        }
        int lastIndex = insidePool.Count - 1;
        Object obj = insidePool[lastIndex];
        insidePool.RemoveAt(lastIndex);
        outsidePool.Add(obj);
        if (obj is MonoBehaviour)
        {
            MonoBehaviour behaviour = obj as MonoBehaviour;
            behaviour.gameObject.SetActive(true);
        }
        else if (obj is GameObject)
        {
            GameObject go = obj as GameObject;
            go.gameObject.SetActive(true);
        }
        return obj;
    }


    public void Clear()
    {
        while (outsidePool.Count > 0)
        {
            ReturnObject(outsidePool[0]);
        }
    }


    public static GameObjectsPool BuildPool(Object prefab, Transform parent)
    {
        GameObject go = new GameObject("Pool_" + prefab.name);
        GameObjectsPool pool = go.AddComponent<GameObjectsPool>();
        pool.Setup(prefab);
        go.transform.parent = parent;

        return pool;
    }
}

