using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameFramework.Core
{
    public class GameInitializer
    {
        const string PATH_TO_CORE_PREFAB = "_GameFramework/GameManager";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnRuntimeMethodLoad()
        {
            //GameManager gamePrefab = Resources.Load<GameManager>(PATH_TO_CORE_PREFAB);
            //GameManager gameInstance = Object.FindObjectOfType<GameManager>();

            //if (gameInstance == null)
            //{
            //    if (gamePrefab != null)
            //    {
            //        gamePrefab = Resources.Load<GameManager>(PATH_TO_CORE_PREFAB);
            //        gameInstance = Object.Instantiate(gamePrefab);
            //        Object.DontDestroyOnLoad(gameInstance.gameObject);
            //    }
            //    else
            //    {
            //        Debug.LogFormat("Resource at path {0} not found", PATH_TO_CORE_PREFAB);
            //    }
            //}
            GameObject go = new GameObject("GameManager");
            go.AddComponent<GameManager>();
            go.AddComponent<UnityLifecycleListener>();
            Object.DontDestroyOnLoad(go);
            GameContextInstaller.Install();
        }
    }
}