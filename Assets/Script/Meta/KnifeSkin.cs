using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class KnifeSkin : ScriptableObject
{
    [SerializeField] string id;
    [SerializeField] GameObject handle;
    [SerializeField] GameObject blade;
    [SerializeField] Vector3 handleOffset;
    [SerializeField] Vector3 bladeOffset;
    [SerializeField] Vector3 bladeForwardAxis;

    public string Id => id;
    public GameObject Handle => handle;
    public GameObject Blade => blade;
    public Vector3 HandleOffset => handleOffset;
    public Vector3 BladeOffset => bladeOffset;
    public Vector3 BladeForwardAxis => bladeForwardAxis;
}
