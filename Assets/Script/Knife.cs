using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] Collider sliceCollider;
    [SerializeField] LayerMask itemMask;
    [SerializeField] LayerMask groundMask;
    int sliceObjects;

    Rigidbody rigidbody;
    public bool Slicing => sliceObjects > 0;
    public Player Player { get; set; }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }


    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (itemMask == (itemMask | (1 << other.gameObject.layer)))
        {
            sliceObjects++;
        }
    }



    private void OnTriggerStay(Collider other)
    {
        ISlicable slicable = other.GetComponent<ISlicable>();
        if(slicable != null)
        {
            float penetration = sliceCollider.bounds.max.z - other.bounds.min.z;
            bool sliced = slicable.TryToSlice(penetration);
            if(sliced)
            {
                sliceObjects--;
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisionGO = collision.collider.gameObject;
        if (groundMask == (groundMask | (1 << collisionGO.gameObject.layer)))
        {
            if (Vector3.Dot(collision.contacts[0].normal, Vector3.back) > 0.5f)
            {
                Kill();
            }
        }
    }


    public void Kill()
    {
        Destroy(GetComponent<Joint>());
        Player.RemoveKnife(this);
        rigidbody.constraints = RigidbodyConstraints.None;
        GetComponent<Collider>().material = null;
        Destroy(this);
    }
}
