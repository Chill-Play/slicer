using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceUISimpleNode : MonoBehaviour
{
    [SerializeField] List<UIAnimationComponent> animationSequence = new List<UIAnimationComponent>();
    [SerializeField] bool zeroScaleOnStart = false;

    private void OnEnable()
    {
        if (zeroScaleOnStart)
        {
            GetComponent<RectTransform>().localScale = Vector3.zero;
        }
        PlayAnimation(0);
    }

    void PlayAnimation(int index)
    {
        if (index < animationSequence.Count)
        {
            animationSequence[index].Play(() => PlayAnimation(index + 1));
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < animationSequence.Count; i++)
        {
            animationSequence[i].Stop(true);
        }
    }
}
