using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Water : MonoBehaviour
{
    struct WaterBody
    {
        public Rigidbody rigidbody;
        public Collider collider;
    }
    [SerializeField] float density = 0.2f;
    [SerializeField] float upwardForce = 2f;
    [SerializeField] ParticleSystem vfx;
    
    Collider trigger;
    List<WaterBody> bodies = new List<WaterBody>();
    // Start is called before the first frame update
    void Start()
    {
        trigger = GetComponent<Collider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for(int i = 0; i < bodies.Count; i++)
        {
            if(bodies[i].collider == null)
            {
                continue;
            }
            Rigidbody rb = bodies[i].rigidbody;
            float depthDiff = trigger.bounds.max.y - bodies[i].collider.bounds.min.y;

            Vector3 friction = -rb.velocity * density;
            rb.AddForce(friction * rb.mass);
            rb.AddTorque(-rb.angularVelocity * rb.mass * density);
            rb.AddForceAtPosition(Vector3.up * depthDiff * rb.mass * upwardForce, rb.transform.TransformPoint(rb.centerOfMass));
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.attachedRigidbody != null)
        {
            WaterBody body = new WaterBody();
            Vector3 point = other.ClosestPoint(trigger.bounds.center);
            if (vfx != null)
            {
                Instantiate(vfx, point, Quaternion.identity);
            }
            body.collider = other;
            body.rigidbody = other.attachedRigidbody;
            bodies.Add(body);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            bodies.RemoveAll((x) => x.collider == other);
        }
    }
}
