using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicedObjectNoParts : MonoBehaviour, ISlicable
{
    public event Action OnSlice;

    [SerializeField] float maxPenetration = 1f;

    public bool TryToSlice(float penetration, Vector3 pos, Vector3 knifeRight)
    {
        if (penetration > maxPenetration)
        {
            OnSlice?.Invoke();
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
