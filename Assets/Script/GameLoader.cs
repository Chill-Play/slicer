using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    [SerializeField] LevelSequence levelSequence;
    [SerializeField] SceneReference tutorial;
    // Start is called before the first frame update
    void Start()
    {
        int levelNumber = PlayerPrefs.GetInt(LevelController.LEVEL_NUMBER_PREFS, 0);
        if(levelNumber == 0)
        {
            SceneManager.LoadScene(tutorial);
            return;
        }
        SceneReference scene = levelSequence.GetScene(levelNumber - 1);
        SceneManager.LoadScene(scene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
