using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    [SerializeField] LevelSequence levelSequence;
    // Start is called before the first frame update
    void Start()
    {
        SceneReference scene = levelSequence.GetScene(PlayerPrefs.GetInt(LevelController.LEVEL_NUMBER_PREFS, 0));
        SceneManager.LoadScene(scene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
