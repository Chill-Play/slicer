using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "level_sequence", menuName = "Level Sequence")]
public class LevelSequence : ScriptableObject
{
    [SerializeField] List<SceneReference> scenes;

    public int Count => scenes.Count;

    public SceneReference GetScene(int i)
    {
        return scenes[i % scenes.Count];
    }
}
