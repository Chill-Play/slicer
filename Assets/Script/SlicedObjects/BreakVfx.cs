using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakVfx : MonoBehaviour
{
    [SerializeField] ParticleSystem breakVfx;
    // Start is called before the first frame update
    void OnEnable()
    {
        GetComponent<ISlicable>().OnSlice += BreakVfx_OnSlice;
    }


    private void OnDisable()
    {
        //GetComponent<SlicedObject>().OnSlice -= BreakVfx_OnSlice;
    }

    private void BreakVfx_OnSlice()
    {
        ParticleSystem vfx = Instantiate(breakVfx, transform.position, Quaternion.identity);
        ParticleSystem.MainModule main = vfx.main;
        SlicedObject slicedObject = GetComponent<SlicedObject>(); // Refactor
        if (slicedObject != null)
        {
            main.startColor = GetComponent<SlicedObject>().Color;
        }
        //vfx. = main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
