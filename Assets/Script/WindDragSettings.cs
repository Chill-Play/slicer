using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wind Drag Settings", menuName = "Wind Drag Settings")]
public class WindDragSettings : ScriptableObject
{
    public Vector3 dragAxis;
    public float dragPower;
}
