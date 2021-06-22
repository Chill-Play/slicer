using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIAnimationComponent : MonoBehaviour
{
    protected Sequence sequence;

    System.Action onAnimationEndCallback;

    public virtual void Play(System.Action onAnimationEndCallback = null)
    {
        sequence = DOTween.Sequence();
        this.onAnimationEndCallback = onAnimationEndCallback;       
    }

    public virtual void Stop(bool copmlete = false)
    {
        if (sequence != null)
        {
            sequence.Kill(copmlete);
        }      
        onAnimationEndCallback = null;
    }

    protected void OnAnimationEnd()
    {    
        sequence = null;
        System.Action temp = onAnimationEndCallback;
        onAnimationEndCallback?.Invoke();
        onAnimationEndCallback -= temp;
    }
}
