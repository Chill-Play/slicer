using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject gameScreen;
    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject winScreen;
    GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        gameController.OnGameStart += GameController_OnGameStart;
        gameController.OnWin += GameController_OnWin;
        gameController.OnLose += GameController_OnLose;
    }

    private void GameController_OnLose()
    {
        gameScreen.gameObject.SetActive(false);
        loseScreen.gameObject.SetActive(true);
    }

    private void GameController_OnWin()
    {
        gameScreen.gameObject.SetActive(false);
        winScreen.gameObject.SetActive(true);
    }

    private void GameController_OnGameStart()
    {
        startScreen.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
