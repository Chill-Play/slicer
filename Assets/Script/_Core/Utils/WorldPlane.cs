using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WorldPlane
{
    public Vector3 origin;
    public Quaternion rotation;
    public Vector3 size;

    public WorldPlane(Vector3 origin, Quaternion rotation, Vector3 size)
    {
        this.origin = origin;
        this.rotation = rotation;
        this.size = size;
    }


    public Matrix4x4 GetTRS()
    {
        return Matrix4x4.TRS(origin, rotation, size);
    }
}
