using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    Vector3 currentVelocity;
    void Start()
    {
        
    }


    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref currentVelocity, 0.2f);
    }


    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
