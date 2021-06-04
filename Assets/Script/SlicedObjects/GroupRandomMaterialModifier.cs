using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupRandomMaterialModifier : MonoBehaviour, ISlicedObjectsGroupModifier
{
    [SerializeField] Material[] materials;
    public void OnSpawn(SpawnInfo info)
    {
        info.spawnedObject.GetComponent<SlicedObjectMaterialSetter>().SetMaterial(materials[Random.Range(0, materials.Length)]);
    }
}
