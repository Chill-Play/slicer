using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Core;


public class UINode : MonoBehaviour, IObject
{ 
    [SerializeField] protected SubjectId subjectId;
    [SerializeField] protected List<SubjectId> nodeTags = new List<SubjectId>();

    bool isEnabled = false;

    public SubjectId SubjectId => subjectId;
    public List<SubjectId> NodeTags => nodeTags;
    public bool Enabled { get => isEnabled; set { isEnabled = value; } }

    public bool HasNodeTag(SubjectId tag)
    {
        return nodeTags.Contains(tag);
    }

    protected virtual void Awake()
    {

    }

    public virtual void ActivateNode()
    {
        gameObject.SetActive(true);
        isEnabled = true;
    }

    public virtual void DeactivateNode()
    {
        gameObject.SetActive(false);
        isEnabled = false;
    }
}
