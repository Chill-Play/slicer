using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIPunchScaleAnimation : UIAnimationComponent
{
    [SerializeField] public float scaleFactor = 0.15f;
    [SerializeField] public float duration = 0.2f;
    [SerializeField] public int vibrato = 10;
    [SerializeField] public float elasticity = 1f;

    public override void Play(Action onAnimationEndCallback = null)
    {
        base.Play(onAnimationEndCallback);

        sequence.Append(GetComponent<RectTransform>().DOPunchScale(Vector3.one * scaleFactor, duration, vibrato, elasticity));
        sequence.AppendCallback(OnAnimationEnd);
    }
}
