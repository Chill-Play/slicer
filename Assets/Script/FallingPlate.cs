using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlate : MonoBehaviour
{
    private Rigidbody _rb;
        // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other) 
    {
        _rb.isKinematic = false;
        Destroy(gameObject, 4f);
    }
}
