using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SlicedObject : MonoBehaviour, ISlicable
{
    public static event System.Action OnSliceGlobal;
    [SerializeField] float sliceVelocity;
    [SerializeField] float penetrationRotation = 30f;
    [SerializeField] float maxPenetration = 1f;
    [SerializeField] float randomAngularVelocityMultiplier = 0.0f;
    [SerializeField] float sliceAngularVelocity;
    [SerializeField] Rigidbody rightPart;
    [SerializeField] Rigidbody leftPart;
    [SerializeField] bool inverseLeftPartVelocity;

    public event Action OnSlice;

    public float SliceVelocityPower { get; set; } = 1.0f;
    public Color Color { get; set; }  = Color.white;


    void Awake()
    {
        rightPart.isKinematic = true;
        leftPart.isKinematic = true;
    }
    

    public bool TryToSlice(float penetration, Vector3 pos, Vector3 knifeRight)
    {
        bool sliced = false;
        if (penetration > maxPenetration)
        {
            PushPart(rightPart, rightPart.transform.right);
            PushPart(leftPart, (inverseLeftPartVelocity) ? -leftPart.transform.right : leftPart.transform.right);
            Destroy(this);
            sliced = true;
            OnSlice?.Invoke();
            OnSliceGlobal?.Invoke();            
        }
        else
        {
            float rotation = Mathf.Lerp(0f, penetrationRotation, penetration / maxPenetration);
            rightPart.transform.localEulerAngles = rightPart.transform.localEulerAngles.SetY(rotation);
            leftPart.transform.localEulerAngles = leftPart.transform.localEulerAngles.SetY(-rotation);
        }
        return sliced;
    }


    void PushPart(Rigidbody part, Vector3 direction)
    {
        Vector3 force = direction * sliceVelocity * SliceVelocityPower;
        force += Vector3.up * sliceVelocity * UnityEngine.Random.Range(0f, 0.2f) * SliceVelocityPower;
        part.isKinematic = false;
        part.AddForce(force, ForceMode.VelocityChange);
        Vector3 angularVelocity = Vector3.Cross(direction, Vector3.up) * sliceAngularVelocity;
        part.angularVelocity = angularVelocity + UnityEngine.Random.insideUnitSphere * 100 * randomAngularVelocityMultiplier;
        part.transform.parent = null;
        Destroy(part.gameObject, 5.0f);
    }
}
