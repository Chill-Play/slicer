using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public const string LEVEL_NUMBER_PREFS = "G_LevelNumber";

    [SerializeField] LevelSequence sequence;
    int currentLevel = -1;

    public int CurrentLevel => currentLevel;

    void Awake()
    {
        currentLevel = PlayerPrefs.GetInt(LEVEL_NUMBER_PREFS);
    }


    public void NextLevel()
    {
        Debug.Log("NextLevel");
        int level = currentLevel + 1;
        PlayerPrefs.SetInt(LEVEL_NUMBER_PREFS, level);
        SceneReference scene = sequence.GetScene(level);
        SceneManager.LoadScene(scene);
    }
    
}
