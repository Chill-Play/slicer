using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlate : MonoBehaviour
{
    private Rigidbody _rb;
    bool isFalling = false;

       
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
    }

    private void OnCollisionEnter(Collision other) 
    {   
        if (!isFalling && (other.gameObject.GetComponent<Player>() != null || other.gameObject.GetComponent<Knife>() != null))
        {
            isFalling = true;
            _rb.useGravity = true;
            _rb.isKinematic = false;
            Destroy(gameObject, 4f);
        }
    }
}
