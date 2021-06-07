using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoundsHelper
{
    public static Bounds GetBounds(List<GameObject> gameObjects)
    {
        Bounds bounds = gameObjects[0].GetComponent<Collider>().bounds;
        for (int i = 1; i < gameObjects.Count; i++)
        {
            bounds.Encapsulate(gameObjects[i].GetComponent<Collider>().bounds);
        }
        return bounds;
    }

    public static void DrawGizmo(Bounds bounds, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawWireCube(bounds.center, bounds.extents);
    }
}
