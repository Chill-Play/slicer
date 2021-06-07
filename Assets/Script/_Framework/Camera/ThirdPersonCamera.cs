using GameFramework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct CameraView
{
    public float distance;
    public float fov;
    public Vector3 angle;
    public Vector3 offset;


    public CameraView(float distance, float fov, Vector3 angle, Vector3 offset)
    {
        this.distance = distance;
        this.fov = fov;
        this.angle = angle;
        this.offset = offset;
    }


    public static CameraView Lerp(CameraView current, CameraView target, float t)
    {
        CameraView newView = new CameraView();
        newView.distance = Mathf.Lerp(current.distance, target.distance, t);
        newView.fov = Mathf.Lerp(current.fov, target.fov, t);
        newView.angle = Quaternion.Slerp(Quaternion.Euler(current.angle), Quaternion.Euler(target.angle), t).eulerAngles;
        newView.offset = Vector3.Lerp(current.offset, target.offset, t);
        return newView;
    }
}


public class ThirdPersonCamera : Entity<ThirdPersonCamera>
{
    [Header("Controller references")]
    [SerializeField] Camera connectedCamera;
    [SerializeField] Transform cameraPivot;
    [SerializeField] Transform cameraArm;

    [Header("Gameplay")]
    [SerializeField] Transform target;
    [SerializeField] float smoothTime = 0.3f;
    [SerializeField] CameraView defaultView = new CameraView(5, 15, new Vector3(45, 0f, 0f), Vector3.zero);

    CameraView currentView;
    CameraView targetView;
    Vector3 viewVelocity;

    private void Awake()
    {
        currentView = defaultView;
        targetView = currentView;
    }


    private void LateUpdate()
    {
        currentView = CameraView.Lerp(currentView, targetView, Time.deltaTime * 3f);
        ApplyView(currentView);
    }


    void ApplyView(CameraView view)
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref viewVelocity, smoothTime);
        cameraArm.localPosition = view.offset;
        cameraArm.localEulerAngles = view.angle;
        cameraPivot.localPosition = Vector3.forward * -view.distance;
        connectedCamera.fieldOfView = view.fov;
    }


    public void SetView(CameraView view)
    {
        targetView = view;
    }


    public void ResetView()
    {
        targetView = defaultView;
    }
}
