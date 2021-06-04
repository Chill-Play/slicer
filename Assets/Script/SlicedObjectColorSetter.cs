using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicedObjectColorSetter : MonoBehaviour
{
    [SerializeField] MeshRenderer rightPart;
    [SerializeField] MeshRenderer leftPart;
    public void SetColor(Color color)
    {
        rightPart.material.SetColor("_MainColor", color);
        leftPart.material.SetColor("_MainColor", color);
        GetComponent<SlicedObject>().Color = color;
    }
}
