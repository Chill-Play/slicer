using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] bool rotate = true;
    [SerializeField] Camera camera;
    [SerializeField] Transform target;
    Vector3 currentVelocity;

    public Transform Target { get; set; }

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

    public void SetRotate(bool rotate)
    {
        this.rotate = rotate;
    }

    public void ApplyShakeOffset(Vector3 offset)
    {
        camera.transform.localPosition = offset;
    }
}
