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
            targets[i].SetMultiplier(multipliers[i], multipliers[i]);            
        }
        targets[0].SetAsLastFinishTarget();
    }

    public void CalculateLastTarget(int knifeCount)
    {
        int lastTarget = Mathf.Clamp(targets.Length - knifeCount, 0, targets.Length - 1);
        Debug.Log("CalculateLastTarget " + targets[lastTarget].name);
        targets[lastTarget].SetAsLastFinishTarget();
    }

    public void CalculateLastTarget(List<Knife> knifes)
    {
        Debug.Log(knifes.Count);
        for (int i = 0; i < targets.Length; i++)
        {
            int knifesIdx = knifes.Count - 1 - i;
            int targetIdx = targets.Length - 1 - i;
            if (knifesIdx >= 0)
            {
                knifes[knifesIdx].SetFinishTarget(targets[targetIdx]);
                if (knifesIdx == 0)
                {
                    Debug.Log("LastFinishTarget " + targets[targetIdx].name);
                    targets[targetIdx].SetAsLastFinishTarget();
                    break;
                }
            }
            if (i == (targets.Length - 1))
            {
                targets[targetIdx].SetAsLastFinishTarget();
                Debug.Log("LastFinishTarget END " + targets[targetIdx].name);
            }
        }
    }
}
