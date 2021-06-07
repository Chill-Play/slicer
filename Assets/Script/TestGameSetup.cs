using GameFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameSetup : MonoBehaviour
{
    [SerializeField] DataId inputId;
    void Start()
    {
        GameplayService gameplayService = IoCContainer.Get<GameplayService>();
        gameplayService.AddGameplayModule(new MovePlayerModule(inputId));
        gameplayService.AddGameplayModule(new KnifeSpawningModule());
        gameplayService.AddGameplayModule(new KillPlayerModule());
    }
}
