using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicedObjectMaterialSetter : MonoBehaviour
{
    [SerializeField] MeshRenderer rightPart;
    [SerializeField] MeshRenderer leftPart;
    [SerializeField] int materialIndex = 0;
    public void SetMaterial(Material material)
    {
        SetMaterial(rightPart, material);
        SetMaterial(leftPart, material);
    }


    void SetMaterial(MeshRenderer renderer, Material material)
    {
        Material[] sharedMaterials = renderer.sharedMaterials;
        sharedMaterials[materialIndex] = material;
        renderer.sharedMaterials = sharedMaterials;
    }
}
