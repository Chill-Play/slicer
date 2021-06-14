using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindDrag : MonoBehaviour
{
    public WindDragSettings Settings { get; set; }

    Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        Vector3 worldDragAxis = transform.TransformDirection(Settings.dragAxis);
        float dot = Vector3.Dot(rigidbody.velocity, worldDragAxis);
        float dragCoeff = Mathf.Abs(dot);

        Vector3 dragForce = -rigidbody.velocity * dragCoeff * Settings.dragPower;
        rigidbody.AddForce(dragForce, ForceMode.Acceleration);
    }
}
