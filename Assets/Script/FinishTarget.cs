using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinishTarget : MonoBehaviour
{
    public static event System.Action<int> OnUpdateMultiplier;
    [SerializeField] TMP_Text text;
    int multiplier;
    public int Multiplier => multiplier;


    private void Start()
    {
        GetComponent<SlicedObject>().OnSlice += FinishTarget_OnSlice;
    }

    private void FinishTarget_OnSlice()
    {
        text.gameObject.SetActive(false);
        OnUpdateMultiplier?.Invoke(multiplier);
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        for(int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Play();
        }
    }

    public void SetMultiplier(int multiplier, int textMult)
    {
        Debug.Log(multiplier + " : " + textMult);
        text.text = "x" + textMult;
        this.multiplier = multiplier;
    }
}
