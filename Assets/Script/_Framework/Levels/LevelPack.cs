using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LevelSettings
{
    public string name;
}


[CreateAssetMenu(menuName = "Game/Level Pack")]
public class LevelPack : ScriptableObject
{
    [SerializeField] List<LevelSettings> levels;

    public int GetLevelsCount()
    {
        return levels.Count;
    }

    public LevelSettings GetLevel(int i)
    {
        return levels[i % levels.Count];
    }
}
