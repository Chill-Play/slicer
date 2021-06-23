using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    public static event System.Action OnSliceGlobal;
    [SerializeField] TMP_Text text;
    [SerializeField] AnimationCurve scaleCurve;
    int score;

    private void OnEnable()
    {
        SlicedObject.OnSliceGlobal += SlicedObject_OnSliceGlobal;
        SlicedMoney.OnSliceGlobal += SlicedObject_OnSliceGlobal;
    }


    private void OnDisable()
    {
        SlicedObject.OnSliceGlobal -= SlicedObject_OnSliceGlobal;
        SlicedMoney.OnSliceGlobal -= SlicedObject_OnSliceGlobal;
    }

    private void SlicedObject_OnSliceGlobal()
    {
        score++;
        text.text = score.ToString();
        StartCoroutine(Scale());
    }

    IEnumerator Scale()
    {
        float t = 0f;
        while (t < 1f)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime * 5f;
            transform.localScale = Vector3.one * scaleCurve.Evaluate(t);
        }
    }
}
