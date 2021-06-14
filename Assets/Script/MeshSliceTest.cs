using EzySlice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSliceTest : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.SliceInstantiate(transform.position, Vector3.right, GetComponent<MeshRenderer>().sharedMaterial);
            //GameObject.Slice( Slicer.Slice(gameObject, new EzySlice.Plane(transform.position, Vector3.up), new TextureRegion(), GetComponent<MeshRenderer>().sharedMaterial);
        }
    }
}
