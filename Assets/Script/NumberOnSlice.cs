using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberOnSlice : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ISlicable>().OnSlice += NumberOnSlice_OnSlice;       
    }

    private void NumberOnSlice_OnSlice()
    {
        UINumbers uINumbers = FindObjectOfType<UINumbers>();
        if (uINumbers != null)
        {
            uINumbers.SpawnNumber(transform.position + offset);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
