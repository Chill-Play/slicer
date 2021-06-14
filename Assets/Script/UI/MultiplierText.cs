using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MultiplierText : MonoBehaviour
{
    [SerializeField] AnimationCurve scaleCurve;
    // Start is called before the first frame update
    void Start()
    {
        FinishTarget.OnUpdateMultiplier += FinishTarget_OnUpdateMultiplier;
    }

    private void FinishTarget_OnUpdateMultiplier(int obj)
    {
        GetComponent<TMP_Text>().text = "x" + obj.ToString();
        StartCoroutine(Scale());
    }

    IEnumerator Scale()
    {
        float t = 0f;
        while(t < 1f)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime * 5f;
            transform.localScale = Vector3.one * scaleCurve.Evaluate(t);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
