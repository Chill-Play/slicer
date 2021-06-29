using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    float levelTime;
    bool gameStarted;
    // Start is called before the first frame update
    void OnEnable()
    {
        GameController gameController = FindObjectOfType<GameController>();
        gameController.OnGameStart += AnalyticsManager_OnGameStart;
        gameController.OnLose += GameController_OnLose;
        gameController.OnWin += GameController_OnWin;
    }


    private void Update()
    {
        levelTime += Time.deltaTime;
    }


    private void GameController_OnWin()
    {
        Dictionary<string, object> event_params = new Dictionary<string, object>();
        event_params.Add("level_time", levelTime);
        AnalyticsEvent.LevelComplete(FindObjectOfType<LevelController>().CurrentLevel, event_params);
        gameStarted = true;
    }

    private void GameController_OnLose()
    {
        Dictionary<string, object> event_params = new Dictionary<string, object>();
        event_params.Add("level_time", levelTime);
        AnalyticsEvent.LevelFail(FindObjectOfType<LevelController>().CurrentLevel, event_params);
    }

    private void AnalyticsManager_OnGameStart()
    {
        AnalyticsEvent.LevelStart(FindObjectOfType<LevelController>().CurrentLevel);
    }

    private void OnDisable()
    {
        GameController gameController = FindObjectOfType<GameController>();
        gameController.OnGameStart -= AnalyticsManager_OnGameStart;
        gameController.OnLose -= GameController_OnLose;
        gameController.OnWin -= GameController_OnWin;
    }


}
