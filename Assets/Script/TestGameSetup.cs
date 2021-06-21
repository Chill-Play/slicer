using GameFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameSetup : MonoBehaviour
{
    [SerializeField] DataId inputId;


    private void Awake()
    {
        FindObjectOfType<GameController>().OnGameStart += TestGameSetup_OnGameStart;
    }

    private void TestGameSetup_OnGameStart()
    {
        StartGame();
    }

    void StartGame()
    {
        GameplayService gameplayService = IoCContainer.Get<GameplayService>();
        //gameplayService.AddGameplayModule(new MovePlayerModule(inputId));
        //gameplayService.AddGameplayModule(new KnifeSpawningModule());
        //gameplayService.AddGameplayModule(new KillPlayerModule());
    }
}
