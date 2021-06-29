using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public struct LevelInfo
{
    public int levelNumber;
    public int levelId;
    public int loop;
    public string levelName;

}


public class LevelController : MonoBehaviour
{
    public const string LEVEL_NUMBER_PREFS = "G_LevelNumber";

    [SerializeField] LevelSequence sequence;
    int currentLevel = -1;
    int levelId;
    int loop;
    string levelName;

    public int CurrentLevel => currentLevel;

    void Awake()
    {
        currentLevel = PlayerPrefs.GetInt(LEVEL_NUMBER_PREFS);
        levelId = ((currentLevel) % sequence.Count) + 1;
        levelName = SceneManager.GetActiveScene().name;
        loop = currentLevel % sequence.Count;
    }


    public LevelInfo GetLevelInfo()
    {
        LevelInfo info = new LevelInfo();
        info.levelId = levelId;
        info.levelNumber = currentLevel + 1;
        info.loop = loop;
        info.levelName = levelName;
        return info;
    }


    public void NextLevel()
    {
        int level = currentLevel + 1;
        PlayerPrefs.SetInt(LEVEL_NUMBER_PREFS, level);
        SceneReference scene = sequence.GetScene(level - 1);
        SceneManager.LoadScene(scene);
    }
    
}
