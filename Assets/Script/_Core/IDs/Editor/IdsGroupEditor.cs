using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(IdsGroup))]
public class IdsGroupEditor : Editor
{   

    public override void OnInspectorGUI()
    {       
        DrawDefaultInspector();

        IdsGroup idsGroup = target as IdsGroup;

        GUILayout.BeginVertical();

        GUILayout.Label("SubjectIds:");

        GUILayout.BeginVertical();

        for (int i = 0; i < idsGroup.SubjectIds.Count; i++)
        {
            if (idsGroup.SubjectIds[i] == null)
            {
                idsGroup.SubjectIds.RemoveAt(i);
                i--;
                continue;
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label( i.ToString() + ". ", GUILayout.Width(30f));
            idsGroup.SubjectIds[i].name = GUILayout.TextField(idsGroup.SubjectIds[i].name);           
            if (GUILayout.Button("-", GUILayout.Width(40f)))
            {
                AssetDatabase.RemoveObjectFromAsset(idsGroup.SubjectIds[i]);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            if (GUILayout.Button("R", GUILayout.Width(40f)))
            {   
                AssetDatabase.ClearLabels(idsGroup.SubjectIds[i]);                
                AssetDatabase.SetLabels(idsGroup.SubjectIds[i], new string[] { idsGroup.SubjectIds[i].name });
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(idsGroup.SubjectIds[i]));

            }
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();


        if (GUILayout.Button("Add SubjectId"))
        {
            SubjectId subjectId = ScriptableObject.CreateInstance<SubjectId>();
            subjectId.name = "testStuff";   
            AssetDatabase.AddObjectToAsset(subjectId, idsGroup);
            idsGroup.AddSubjectId(subjectId);
            AssetDatabase.SaveAssets();            
        }

        GUILayout.EndVertical();
    }

}
