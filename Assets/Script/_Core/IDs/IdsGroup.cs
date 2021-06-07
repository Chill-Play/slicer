using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ids_group", menuName = "HCFramework/IdsGroup")]
public class IdsGroup : ScriptableObject
{
   [SerializeField, HideInInspector] List<SubjectId> subjectIds = new List<SubjectId>();

    public List<SubjectId> SubjectIds => subjectIds;

    public void AddSubjectId(SubjectId subjectId)
    {
        subjectIds.Add(subjectId);
    }
}
