using EzySlice;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class SlicedObjectPartCreator : MonoBehaviour, ISlicable
{
    public static event System.Action OnSliceGlobal;
    [SerializeField] float sliceVelocity;
    [SerializeField] float penetrationRotation = 30f;
    [SerializeField] float maxPenetration = 1f;
    [SerializeField] float randomAngularVelocityMultiplier = 0.0f;
    [SerializeField] float sliceAngularVelocity;
    [SerializeField] WindDragSettings dragSettings;
    [SerializeField] bool addScore = true;
    
    Rigidbody rightPart;
    Rigidbody leftPart;
    [SerializeField] bool inverseLeftPartVelocity;
    bool partsCreated;
    public event Action OnSlice;

    public float SliceVelocityPower { get; set; } = 1.0f;
    public Color Color { get; set; }  = Color.white;
    public bool Unsliceable { get; set; } = false;


    public bool TryToSlice(float penetration, Vector3 pos, Vector3 knifeRight)
    {
        if (Unsliceable)
        {
            OnSlice?.Invoke();
            return false;
        }
        bool sliced = false;

        if(!partsCreated)
        {
            GameObject[] parts = gameObject.SliceInstantiate(pos, knifeRight, GetComponentInChildren<MeshRenderer>().material);
            if (parts != null && parts.Length > 0)
            {
                GetComponentInChildren<MeshRenderer>().enabled = false;
                rightPart = parts[0].AddComponent<Rigidbody>();
                rightPart.transform.position = transform.position;
                rightPart.transform.parent = transform;

                leftPart = parts[1].AddComponent<Rigidbody>();
                leftPart.transform.position = transform.position;
                rightPart.isKinematic = true;
                leftPart.isKinematic = true;
                leftPart.transform.parent = transform;

                rightPart.gameObject.layer = 14;
                leftPart.gameObject.layer = 14;

                rightPart.gameObject.AddComponent<BoxCollider>();
                leftPart.gameObject.AddComponent<BoxCollider>();
                if(dragSettings != null)
                {
                    rightPart.gameObject.AddComponent<WindDrag>().Settings = dragSettings;
                    leftPart.gameObject.AddComponent<WindDrag>().Settings = dragSettings;
                }
                partsCreated = true;
            }
        }
        if (partsCreated)
        {
            if (maxPenetration == 0f || penetration > maxPenetration)
            {
                PushPart(rightPart, transform.right);
                PushPart(leftPart, -transform.right);
                Destroy(this);
                GetComponent<Collider>().enabled = false;
                sliced = true;
                OnSlice?.Invoke();
                if (addScore)
                {
                    OnSliceGlobal?.Invoke();
                }
            }
            else
            {
                float rotation = Mathf.Lerp(0f, penetrationRotation, penetration / maxPenetration);
                rightPart.transform.localEulerAngles = rightPart.transform.localEulerAngles.SetY(rotation);
                leftPart.transform.localEulerAngles = leftPart.transform.localEulerAngles.SetY(-rotation);
            }
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
