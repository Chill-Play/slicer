using GameFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameSetup : MonoBehaviour
{
    [SerializeField] DataId inputId;

    List<GameplayModule> gameplayModules = new List<GameplayModule>();
    GameplayService gameplayService;
    public static TestGameSetup instance;

    void Start()
    {
        instance = this;

        gameplayService = IoCContainer.Get<GameplayService>();

        gameplayModules.Add(new MovePlayerModule(inputId));
        gameplayModules.Add(new KnifeSpawningModule());
        gameplayModules.Add(new KillPlayerModule());

        for (int i = 0; i < gameplayModules.Count; i++)
        {
            gameplayService.AddGameplayModule(gameplayModules[i]);            
        }
    }

    public void RemoveGameplayModule<T>()
    {
        for (int i = 0; i < gameplayModules.Count; i++)
        {
            if (gameplayModules[i] is T)
            {
                gameplayService.RemoveGameplayModule(gameplayModules[i]);
            }
        }
    }
}
