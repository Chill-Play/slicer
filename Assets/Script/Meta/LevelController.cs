using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public const string LEVEL_NUMBER_PREFS = "G_LevelNumber";

    [SerializeField] LevelSequence sequence;
    int currentLevel = -1;


    void Awake()
    {
        currentLevel = PlayerPrefs.GetInt(LEVEL_NUMBER_PREFS);
    }


    void NextLevel()
    {
        int level = currentLevel + 1;
        PlayerPrefs.SetInt(LEVEL_NUMBER_PREFS, level);
        sequence.GetScene(level);
    }
    
}
