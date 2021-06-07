using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class VfxPoolObject : MonoBehaviour, IPoolObject
{
    public int PrefabId { get; set; }

    public void OnParticleSystemStopped()
    {
        Spawner.ReturnObject(PrefabId, gameObject);
    }

    public void OnReturnToPool()
    {

    }
}
