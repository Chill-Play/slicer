using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] bool rotate = true;
    [SerializeField] Camera camera;
    [SerializeField] Transform target;
    [SerializeField] float minY = -5f;
    Vector3 currentVelocity;
    bool stopOnMin = false;

    public Transform Target { get; set; }

    void Start()
    {
        FindObjectOfType<GameController>().OnLose += CameraController_OnLose; 
    }

    private void CameraController_OnLose()
    {
        stopOnMin = true;
    }

    void LateUpdate()
    {
        if (stopOnMin)
        {
            if (transform.position.y < minY)
            {
                target = null;
            }
        }
        if (target != null)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref currentVelocity, 0.2f);
        }
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
