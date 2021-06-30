using GameFramework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    float levelStartTime;
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
        SendLevelCompleted(FindObjectOfType<LevelController>().GetLevelInfo());
        gameStarted = true;
    }

    private void GameController_OnLose()
    {
        Dictionary<string, object> event_params = new Dictionary<string, object>();
        event_params.Add("level_time", levelTime);
        SendLevelFailed(FindObjectOfType<LevelController>().GetLevelInfo());
        AnalyticsEvent.LevelFail(FindObjectOfType<LevelController>().CurrentLevel, event_params);
    }

    private void AnalyticsManager_OnGameStart()
    {
        SendLevelStarted(FindObjectOfType<LevelController>().GetLevelInfo());
        AnalyticsEvent.LevelStart(FindObjectOfType<LevelController>().CurrentLevel);
    }

    private void OnDisable()
    {
        GameController gameController = FindObjectOfType<GameController>();
        if (gameController)
        {
            gameController.OnGameStart -= AnalyticsManager_OnGameStart;
            gameController.OnLose -= GameController_OnLose;
            gameController.OnWin -= GameController_OnWin;
        }
    }


    public void SendLevelStarted(LevelInfo info)
    {
        Dictionary<string, object> p = new Dictionary<string, object>();
        AddLevelInfoToParams(info, p);
        ReportEvent("level_start", p);
        AppMetrica.Instance.SendEventsBuffer();
    }



    void AddLevelInfoToParams(LevelInfo info, Dictionary<string, object> p)
    {
        p.Add("level_number", info.levelId);
        p.Add("level_name", info.levelName);
        p.Add("level_count", info.levelNumber);
        p.Add("level_diff", "medium");
        p.Add("level_loop", info.loop);
        p.Add("level_random", 0);
        p.Add("level_type", "normal");
        p.Add("game_mode", "classic");
    }


    void AddLevelInfoFinishParams(LevelInfo info, Dictionary<string, object> p)
    {
        p.Add("time", (int)(Time.time - levelStartTime));
        p.Add("progress", (int)(IoCContainer.Get<EntityService>().GetFirstEntity<Player>().Progress * 100));
    }



    public void SendLevelFailed(LevelInfo info)
    {
        Dictionary<string, object> p = new Dictionary<string, object>();
        AddLevelInfoToParams(info, p);
        p.Add("result", "lose");
        AddLevelInfoFinishParams(info, p);
        ReportEvent("level_finish", p);
        AppMetrica.Instance.SendEventsBuffer();
    }


    public void SendLevelCompleted(LevelInfo info)
    {
        Dictionary<string, object> p = new Dictionary<string, object>();
        AddLevelInfoToParams(info, p);
        p.Add("result", "win");
        AddLevelInfoFinishParams(info, p);
        ReportEvent("level_finish", p);
        AppMetrica.Instance.SendEventsBuffer();
    }


    public void ReportEvent(string id, Dictionary<string, object> parameters)
    {
        Debug.Log("Analytics event : " + id + " : " + string.Join(Environment.NewLine, parameters));
        AppMetrica.Instance.ReportEvent(id, parameters);
    }


}
