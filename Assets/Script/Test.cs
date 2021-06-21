using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Test : MonoBehaviour
{
    [SerializeField] int size = 10000;
    object array;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            array = new float[size];
            UnityEngine.Debug.Log("float : " + stopwatch.Elapsed);
        }

        for (int i = 0; i < 10; i++)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            array = new byte[size];
            UnityEngine.Debug.Log("byte : " + stopwatch.Elapsed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
