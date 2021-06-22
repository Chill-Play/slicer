using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIAnimationJoinType
{
    Join,
    Append
}

[System.Serializable]
public struct UIAnimationJoinStruct
{
    public UIAnimationSettings animation;
    public UIAnimationJoinType joinType;
    [System.NonSerialized] public System.Action onAnimationEndCallback;

    public UIAnimationJoinStruct(UIAnimationSettings animation, UIAnimationJoinType joinType, System.Action onAnimationEndCallback)
    {
        this.animation = animation;
        this.joinType = joinType;
        this.onAnimationEndCallback = onAnimationEndCallback;
    }

    public void AddOnAnimationEndCallback(System.Action onAnimationEndCallback)
    {
        this.onAnimationEndCallback += onAnimationEndCallback;
    }
}




public class UIAnimationSequence 
{
    List<UIAnimationJoinStruct> animationSequence = new List<UIAnimationJoinStruct>();
    List<UIAnimation> runingAnimations = new List<UIAnimation>();

    int nextAnimationIndex = 0;

    System.Action onSequenceEnd;

    UIScreen screen;

    public UIAnimationSequence(UIScreen screen, List<UIAnimationJoinStruct> animationsSequence, System.Action onSequenceEnd = null)
    {
        this.screen = screen;
        for (int i = 0; i < animationsSequence.Count; i++)
        {       
            animationSequence.Add(animationsSequence[i]);
        }
        this.onSequenceEnd += onSequenceEnd;
    }

    public void Run()
    {
        TryRunNextAnimation();
    }

    public void Stop()
    {
        for (int i = 0; i < runingAnimations.Count; i++)
        {
            runingAnimations[i].Stop();
        }
    }

    void TryRunNextAnimation()
    {
        if (nextAnimationIndex < animationSequence.Count)
        {           
            switch (animationSequence[nextAnimationIndex].joinType)
            {
                case UIAnimationJoinType.Append:
                    if (runingAnimations.Count == 0)
                    {
                        RunNextAnimation();
                    }
                    break;
                case UIAnimationJoinType.Join:
                    RunNextAnimation();
                    break;
            }
        }
        else if (runingAnimations.Count  == 0)
        {         
            onSequenceEnd.Invoke();
        }
    }

    void RunNextAnimation()
    {
        //UIAnimation animation = animationSequence[nextAnimationIndex].animation.GetAnimation();
        //animation.AddOnAnimationEndCallback(animationSequence[nextAnimationIndex].onAnimationEndCallback);
        //runingAnimations.Add(animation);
        //UINodesManager.Instance.Play(screen, animation, () => OnAnimationEnd(animation));
        //nextAnimationIndex++;
        TryRunNextAnimation();
    }

    public void OnAnimationEnd(UIAnimation animation)
    {
        runingAnimations.Remove(animation);
        TryRunNextAnimation();
    }

    public UIAnimationJoinStruct this[int key]
    {
        get => animationSequence[key];        
    }
}

public class UINodesManager : MonoBehaviour
{
   [SerializeField] Dictionary<SubjectId, UINode> nodes = new Dictionary<SubjectId, UINode>();

    Dictionary<UIScreen, UIAnimationSequence> animationsSequences = new Dictionary<UIScreen, UIAnimationSequence>();


    private void Awake()
    {
        UINode[] nodesComponents = GetComponentsInChildren<UINode>();

        for (int i = 0; i < nodesComponents.Length; i++)
        {
            if (!nodes.ContainsKey(nodesComponents[i].SubjectId))
            {
                nodes.Add(nodesComponents[i].SubjectId, nodesComponents[i]);               
            }
            else
            {
                Debug.Log("UINodes already contains key " + nodesComponents[i].SubjectId.name);
            }
        }
    }

    public UINode GetNode(SubjectId nodeId)
    {
        UINode node;
        if (nodes.TryGetValue(nodeId, out node))
        {          
            return node;
        }

        return null;    
    }


    public List<UINode> GetNodesWithTag(SubjectId tag)
    {
        List<UINode> resNodes = new List<UINode>();
       
        foreach (KeyValuePair<SubjectId, UINode> n  in nodes)
        {
            if (n.Value.HasNodeTag(tag))
            {
                resNodes.Add(n.Value);
            }
        }

        return resNodes;
    }

    public void ActivateScreen(UIScreen screen)
    {
        for (int i = 0; i < screen.NodesIds.Count; i++)
        {
            UINode node = GetNode(screen.NodesIds[i]);
            if (node != null)
            {
                node.ActivateNode();
            }
        }
    }


    public void DeactivateScreen(UIScreen screen)
    {
        for (int i = 0; i < screen.NodesIds.Count; i++)
        {         
            UINode node = GetNode(screen.NodesIds[i]);
            if (node != null)
            {               
                node.DeactivateNode();
            }
        }
    }

    public void Play(UIScreen screen, UIAnimation animation, System.Action onAnimationEndCallback = null)
    {
        List<RectTransform> nodeTransforms = new List<RectTransform>();
        for (int i = 0; i < screen.NodesIds.Count; i++)
        {
            UINode node = GetNode(screen.NodesIds[i]);
            if (node != null && animation.AnimationSettings.HasNodeTag(node))
            {
                RectTransform nodeTransform = node.GetComponent<RectTransform>();
                if (nodeTransform != null)
                {
                    nodeTransforms.Add(nodeTransform);
                }
            }
        }
        animation.Play(nodeTransforms.GetEnumerator(), onAnimationEndCallback);
    }

    public void RunAnimationsSequence(UIScreen screen, List<UIAnimationJoinStruct> animationsSequence, System.Action onSequenceEnd = null)
    {      
        System.Action onSequenceEndTemp = () => OnSequenceEnd(screen);
        onSequenceEndTemp += onSequenceEnd;

        UIAnimationSequence sequence = new UIAnimationSequence(screen, animationsSequence, onSequenceEndTemp);
        if (animationsSequences.ContainsKey(screen))
        {
            animationsSequences[screen].Stop();
        }   
        animationsSequences.Add(screen, sequence);
        sequence.Run();
    }

    public void StopAnimationScreen(UIScreen screen)
    {
        if (animationsSequences.ContainsKey(screen))
        {
            animationsSequences[screen].Stop();
            animationsSequences.Remove(screen);
        }
    }

    void OnSequenceEnd(UIScreen screen)
    {
        animationsSequences.Remove(screen);
    }


    public void ApplyTheme(UITheme theme)
    {
        List<UIThemeSettings> themeSettings = theme.ThemeSettings;
        for (int i = 0; i < themeSettings.Count; i++)
        {
            if (themeSettings[i].changingValue is SubjectId)
            {
                SubjectId changingValue = themeSettings[i].changingValue as SubjectId;
                themeSettings[i].nodeChanger.Apply(GetNodesWithTag(changingValue).GetEnumerator());
            }
            else
            {
                IEnumerable<UINode> node = GetNode(themeSettings[i].changingValue).Yield<UINode>();
                themeSettings[i].nodeChanger.Apply(node.GetEnumerator());
            }
        }
    }
}