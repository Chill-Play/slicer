using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    [SerializeField] TMP_Text label;


    private void Awake()
    {
        label.rectTransform.localScale = Vector3.zero;
    }


    public void Show(string text)
    {
        label.text = text;
        label.transform.DOScale(1f, 0.5f).SetEase(Ease.OutElastic, 1.3f, 0.2f).SetUpdate(UpdateType.Normal, true);
    }


    public void Hide()
    {
        label.transform.DOScale(0f, 0.3f).SetEase(Ease.InCirc).SetUpdate(UpdateType.Normal, true);
    }
}
