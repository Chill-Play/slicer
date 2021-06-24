using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] Animator animator;
    Rigidbody[] rigidbodies;
    // Start is called before the first frame update
    void Start()
    {
        SetRagdollActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetRagdollActive(bool active)
    {
        animator.enabled = !active;
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        for(int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = !active;
        }
    }

    public void Push(Vector3 push, Vector3 point)
    {
        foreach (var rb in rigidbodies)
        {
            rb.AddForceAtPosition(push, point, ForceMode.VelocityChange);
        }
    }
}
