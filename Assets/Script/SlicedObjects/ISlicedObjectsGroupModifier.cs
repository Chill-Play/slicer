using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SpawnInfo { 
    public SlicedObject spawnedObject;
    public int number;
    public int total;
}


public interface ISlicedObjectsGroupModifier
{
    void OnSpawn(SpawnInfo info);
}
