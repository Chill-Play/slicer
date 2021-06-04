using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] Animator animator;
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
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        for(int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = !active;
        }
    }
}
