using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] bool rotate = true;
    [SerializeField] Camera camera;
    [SerializeField] Transform holder;
    Vector3 currentVelocity;
    Quaternion targetRotation;

    public Transform Target { get; set; }

    void Start()
    {
        
    }


    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, Target.position, ref currentVelocity, 0.2f);
        if(rotate)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1f);
        }
    }


    public void SetRotation(Quaternion targetRotation)
    {
        this.targetRotation = targetRotation;
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
