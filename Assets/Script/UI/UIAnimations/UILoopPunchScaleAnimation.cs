using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UILoopPunchScaleAnimation : UIAnimationComponent
{
    [SerializeField] float scaleFactor = 0.15f;
    [SerializeField] float duration = 1.5f;
    [SerializeField] int vibrato = 1;
    [SerializeField] float elasticity = 1f;

    Tweener tweener;

    public override void Play(Action onAnimationEndCallback = null)
    {
        tweener = GetComponent<RectTransform>().DOPunchScale(Vector3.one * scaleFactor, duration, vibrato, elasticity).SetLoops(-1);
    }

    public override void Stop(bool copmlete = false)
    {
        tweener.Kill(copmlete);
    }

}
