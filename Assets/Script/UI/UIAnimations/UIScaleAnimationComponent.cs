using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class UIScaleAnimationComponent : UIAnimationComponent
{
    [SerializeField] float waitUntilFirstAppearance = 0f;
    [SerializeField] float scaleValue = 1f;
    [SerializeField] float scaleTime = 0.5f;
    [SerializeField] float scaleEaseAmplitude = 1.2f;
    [SerializeField] float scaleEasePeriod = 0.5f;
    [SerializeField] Ease easeType = Ease.OutElastic;


    public override void Play(System.Action onAnimationEndCallback = null)
    {
        base.Play(onAnimationEndCallback);

        RectTransform nodeTransform = GetComponent<RectTransform>();
        sequence.AppendInterval(waitUntilFirstAppearance);
        sequence.Append(nodeTransform.DOScale(scaleValue, scaleTime).SetEase(easeType, scaleEaseAmplitude, scaleEasePeriod));       
        sequence.AppendCallback(OnAnimationEnd);       
    }




}
