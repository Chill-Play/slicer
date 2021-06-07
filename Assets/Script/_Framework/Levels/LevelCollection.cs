


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Level Collection")]
public class LevelCollection : ScriptableObject
{
    [SerializeField] List<LevelPack> packs;

    public int GetPacksCount()
    {
        return packs.Count;
    }


    public LevelPack GetPackForLevel(int level, out int levelInPack, out int loop, out int levelId, out string levelName)
    {
        int tempLevel = level;
       
        int pack = 0;
        loop = 1;
        while(tempLevel >= GetPack(pack).GetLevelsCount())
        {
            tempLevel -= GetPack(pack).GetLevelsCount();
            pack++;
            if(pack >= packs.Count)
            {
                loop++;
                pack = 0;
            }
        }
        levelInPack = tempLevel;
        levelId = 0;
        for (int i = 0; i < pack; i++)
        {
            levelId += GetPack(i).GetLevelsCount();  
        }
        levelId += levelInPack + 1;
        levelName = GetPack(pack).GetLevel(levelInPack).name;
        return GetPack(pack);
    }

    public LevelPack GetPack(int i)
    {
        return packs[i];
    }   


    
}
