using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] FinishTarget[] targets;
    [SerializeField] int[] multipliers;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < targets.Length; i++)
        {
            if (i > 0)
            {
                targets[i].SetMultiplier(multipliers[i - 1], multipliers[i]);
            }
            else
            {
                targets[i].SetMultiplier(multipliers[i], multipliers[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
