using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicedMoney : MonoBehaviour, ISlicable
{
    [SerializeField] float sliceVelocity;
    [SerializeField] float maxPenetration = 1f;
    [SerializeField] float randomAngularVelocityMultiplier = 0.0f;
    [SerializeField] float sliceAngularVelocity;
    [SerializeField] float penetrationStep = 0.1f;
    [SerializeField] Rigidbody partPrefab;
    [SerializeField] bool inverseLeftPartVelocity;

    public event System.Action OnSlice;

    public float SliceVelocityPower { get; set; } = 1.0f;
    public Color Color { get; set; } = Color.white;

    float nextPenetration;

    void Awake()
    {

    }


    public bool TryToSlice(float penetration, Vector3 pos, Vector3 knifeRight)
    {
        bool sliced = false;
        if (penetration > maxPenetration)
        {
            Destroy(gameObject);
            sliced = true;
            OnSlice?.Invoke();
        }
        else
        {
            if(penetration > nextPenetration)
            {
                Rigidbody rightPart = Instantiate(partPrefab, transform.position, Quaternion.Euler(-90f,0f, 0f));
                Rigidbody leftPart = Instantiate(partPrefab, transform.position, Quaternion.Euler(-90f, 0f, 0f));
                rightPart.transform.localScale = new Vector3(-rightPart.transform.localScale.x, rightPart.transform.localScale.y, rightPart.transform.localScale.z);
                PushPart(rightPart, rightPart.transform.right);
                PushPart(leftPart, -leftPart.transform.right);
                nextPenetration += penetrationStep;
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - (penetration / maxPenetration), transform.localScale.z);
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
