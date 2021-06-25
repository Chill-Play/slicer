using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinishTarget : MonoBehaviour
{
    public static event System.Action<int> OnUpdateMultiplier;
    public static event System.Action OnLastFinishTargetHitted;
    [SerializeField] TMP_Text text;
    int multiplier;
    public int Multiplier => multiplier;
    bool lastFinishTarget = false;



    private void OnSlice()
    {   
        OnUpdateMultiplier?.Invoke(multiplier);
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        for(int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Play();
        }
        if (lastFinishTarget)
        {
            OnLastFinishTargetHitted?.Invoke();
        }
        else
        {
            GetComponent<SlicedObject>().ForceToSlice();
            text.gameObject.SetActive(false);
        }
    }


    public bool Slice()
    {
        OnSlice();
        return !lastFinishTarget;
    }


    public void SetMultiplier(int multiplier, int textMult)
    {
        Debug.Log(multiplier + " : " + textMult);
        text.text = "x" + textMult;
        this.multiplier = multiplier;
    }

    public void SetAsLastFinishTarget()
    {
        lastFinishTarget = true;
        GetComponent<SlicedObject>().Unsliceable = true;
    }
}
