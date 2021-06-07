using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolObject
{
    int PrefabId { get; set; }

    void OnReturnToPool();
}
