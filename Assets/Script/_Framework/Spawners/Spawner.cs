using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Spawner : MonoBehaviour
{

    #region Singleton

    private static Spawner instance;

    public static Spawner Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion


    public Dictionary<int, GameObjectsPool> objectPoolsById = new Dictionary<int, GameObjectsPool>();


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }


    public static void ReturnObject(int id, Object poolObject)
    {
        if (instance.objectPoolsById.TryGetValue(id, out GameObjectsPool pool))
        {            
            pool.ReturnObject(poolObject);
        }
        else
        {
            Debug.LogError("No pool was created for : " + poolObject.name);
        }
    }


    public static void ReturnObject(Object prefab, Object poolObject)
    {
        ReturnObject(prefab.GetInstanceID(), poolObject);
    }


    public static GameObjectsPool GetPool(Object prefab)
    {
        return instance.objectPoolsById[prefab.GetInstanceID()];
    }



    public static T SpawnPrefab<T>(T prefab) where T : Object
    {
        return SpawnPrefab<T>(prefab, Vector3.zero, Quaternion.identity, null);
    }


    public static T SpawnPrefab<T>(T prefab, Transform parent) where T : Object
    {
        return SpawnPrefab<T>(prefab, parent.position, parent.rotation, parent);
    }


    public static T SpawnPrefab<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object
    {
        return SpawnPrefab<T>(prefab, position, rotation, null);
    }


    public static T SpawnPrefab<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Object
    {
        T result = null;
        int prefabId = prefab.GetInstanceID();

        GameObjectsPool pool;

        if (!instance.objectPoolsById.TryGetValue(prefabId, out pool))
        {
            pool = GameObjectsPool.BuildPool(prefab, instance.transform);
            instance.objectPoolsById.Add(prefabId, pool);
        }

        result = pool.GetObject() as T;
        IPoolObject poolObject = null;
        if (result is Component)
        {
            Component behaviour = result as Component;
            poolObject = behaviour.GetComponent<IPoolObject>();
            Transform transform = behaviour.transform;
            transform.position = position;
            transform.rotation = rotation;
            transform.SetParent(parent);
        }
        else if (result is GameObject)
        {
            GameObject obj = result as GameObject;
            poolObject = obj.GetComponent<IPoolObject>();
            Transform transform = obj.transform;
            transform.position = position;
            transform.rotation = rotation;
            transform.SetParent(parent);
        }

        if (poolObject != null)
        {
            poolObject.PrefabId = prefabId;
        }

        return result;
    }


    public static void Clear()
    {
        foreach (GameObjectsPool pool in instance.objectPoolsById.Values)
        {
            pool.Clear();
        }
    }

}
