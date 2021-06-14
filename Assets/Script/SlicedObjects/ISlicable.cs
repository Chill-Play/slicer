using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISlicable
{
    event System.Action OnSlice;
    bool TryToSlice(float penetration, Vector3 point, Vector3 knifeRight);
}
