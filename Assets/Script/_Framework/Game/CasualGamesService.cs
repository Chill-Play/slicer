using GameFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//[AutoInitializeService]
public class CasualGamesService : IService
{
    CasualGame currentGame;

    public CasualGame CurrentGame => currentGame;
    public bool Enabled { get; set; }


    public void RunGame(CasualGame game)
    {
        currentGame = game;
        //SceneManager.LoadScene(game.mainScene, LoadSceneMode.Single);
    }
}
