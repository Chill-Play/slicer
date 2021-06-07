using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Core
{
    [AutoInitializeService]
    public class GameFlowService : IService
    {
        #region Variables

        List<GameState> statesStack = new List<GameState>();

        #endregion



        #region Properties

        public GameState CurrentState => statesStack[statesStack.Count - 1];

        #endregion



        #region IService

        public bool Enabled { get; set; }

        #endregion



        #region IGameFlow

        public void MoveToState(GameState state)
        {
            SetCurrentState(state);
        }


        public void AppendState(GameState state)
        {
            CurrentState.Pause();
            statesStack.Add(state);
            CurrentState.Initialize();
        }


        public void RemoveActiveState()
        {
            statesStack.RemoveAt(statesStack.Count - 1);
            CurrentState.Resume();
        }


        #endregion



        #region Private methods

        void SetCurrentState(GameState state)
        {
            for (int i = 0; i < statesStack.Count; i++)
            {
                statesStack[i].End();
            }
            statesStack.Clear();
            Debug.Log("GameFlow : Set current stage : " + state.GetType().Name);

            statesStack.Add(state);
            CurrentState.Initialize();
        }

        #endregion
    }
}