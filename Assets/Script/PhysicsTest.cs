﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsTest : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.material.dynamicFriction);
        Debug.Log(collision.collider.material.staticFriction);
        Debug.Log(collision.collider.material.frictionCombine);
    }
}
