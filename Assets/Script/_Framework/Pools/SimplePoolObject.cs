using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SimplePoolObject : MonoBehaviour, IPoolObject
{
    [SerializeField] float timeToReturn = 3;
    public int PrefabId { get; set; }
    float timer;


    void Awake()
    {
        timer = timeToReturn;
    }


    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            if (PrefabId != 0)
            {
                Spawner.ReturnObject(PrefabId, gameObject);
            }
            else
            {
                gameObject.SetActive(false);
                Debug.LogError("Prefab was created not from pool : " + gameObject.name);
            }
        }
    }


    public void OnReturnToPool()
    {

    }
}
