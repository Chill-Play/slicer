using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameFramework.Core
{
    [AutoInitializeService]
    public class GameplayService : IService
    {
        #region Properties

        public List<GameplayModule> GameplayModules { get; private set; } = new List<GameplayModule>();

        #endregion



        #region IService

        public bool Enabled { get; set; }

        #endregion



        #region IGameplaySystem

        public void AddGameplayModule(GameplayModule module)
        {
            if (GameplayModules == null)
            {
                GameplayModules = new List<GameplayModule>();
            }
            GameplayModules.Add(module);
            module.SetActive(true);
            if (module.Active)
            {
                module.Initialize();
            }
            ObjectsLifecycleSystem.Instance.BindLifecycle(module);
        }


        public void RemoveGameplayModule(GameplayModule module)
        {
            module.End();
            GameplayModules.Remove(module);
            ObjectsLifecycleSystem.Instance.UnbindLifecycle(module);
        }


        public void Clear()
        {
            if (GameplayModules != null)
            {
                for (int i = GameplayModules.Count - 1; i >= 0; i--)
                {
                    //RemoveGameplayModule(GameplayModules[i]);//.End();
                    GameplayModules[i].SetActive(false); // refactor
                }
                //GameplayModules.Clear();
            }
        }

        #endregion
    }
}