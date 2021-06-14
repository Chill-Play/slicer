using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] bool rotate = true;
    [SerializeField] Camera camera;
    [SerializeField] Transform holder;
    Vector3 currentVelocity;
    Quaternion targetRotation;

    void Start()
    {
        
    }


    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref currentVelocity, 0.2f);
        if(rotate)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1f);
        }
    }


    public void SetRotation(Quaternion targetRotation)
    {
        this.targetRotation = targetRotation;
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
