using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject gameScreen;
    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject tutorialScreen;
    [SerializeField] TutorialInput tutorialInput;

    GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        gameController.OnGameStart += GameController_OnGameStart;
        gameController.OnWin += GameController_OnWin;
        gameController.OnLose += GameController_OnLose;
        gameController.OnTutorialStart += GameController_OnTutorialStart;
        gameController.OnTutorialEnd += GameController_OnTutorialEnd; ;
    }

    private void GameController_OnTutorialEnd()
    {
       // gameScreen.gameObject.SetActive(true);
        tutorialScreen.gameObject.SetActive(false);
    }

    private void GameController_OnTutorialStart(TutorialPoint.TutorialPointInfo tutorialPointInfo)
    {
        //gameScreen.gameObject.SetActive(false);
        tutorialScreen.gameObject.SetActive(true);
        tutorialInput.Setup(tutorialPointInfo);
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
        gameScreen.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
