using System.Collections.Generic;
using UnityEngine;

public class UIAnimationSettings : ScriptableObject
{
    [SerializeField] protected List<SubjectId> nodeTags = new List<SubjectId>();

    public virtual UIAnimation GetAnimation()
    {
        return null;
    }

    public virtual void Initialize(IEnumerator<RectTransform> nodeTransforms)
    {

    }

    public bool HasNodeTag(UINode node)
    {
        for (int i = 0; i < node.NodeTags.Count; i++)
        {
            if (nodeTags.Contains(node.NodeTags[i]))
            {
                return true;
            }
        }

        return false;
    }


}

public class UIAnimation
{
    protected UIAnimationSettings animationSettings;

    protected System.Action onAnimationEndCallback;

    public UIAnimationSettings AnimationSettings => animationSettings;

    public virtual void Play(IEnumerator<RectTransform> nodeTransforms, System.Action onAnimationEndCallback = null)
    {
        this.onAnimationEndCallback += onAnimationEndCallback;       
    }

    public virtual void Stop()
    {

    }

    public void AddOnAnimationEndCallback(System.Action onAnimationEndCallback)
    {
        this.onAnimationEndCallback += onAnimationEndCallback;
    }

    protected void OnAnimationEnd()
    {    
        onAnimationEndCallback?.Invoke();      
    }
}
