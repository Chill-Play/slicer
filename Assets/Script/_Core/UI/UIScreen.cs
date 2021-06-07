using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ui_screen", menuName = "HCFramework/UIScreen")]
public class UIScreen : ScriptableObject
{
    [SerializeField] List<SubjectId> nodesIds = new List<SubjectId>();

    public List<SubjectId> NodesIds => nodesIds;

}
