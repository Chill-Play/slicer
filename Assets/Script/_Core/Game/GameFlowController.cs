using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameFramework.Core
{
    public class GameFlowController : MonoBehaviour
    {
        #region Variables

        [SerializeField] SubjectId startState;

        List<GameState> statesStack = new List<GameState>();
        List<GameState> statesList = new List<GameState>();

        #endregion



        #region Properties

        public GameState CurrentState => statesStack[statesStack.Count - 1];

        #endregion



        #region IService

        public bool Enabled { get; set; }

        #endregion


        #region Unity Lifecycle

        private void Awake()
        {
            statesList = GetComponentsInChildren<GameState>(true).ToList();
            if(startState != null)
            {
                MoveToState(startState);
            }
        }

        #endregion



        #region IGameFlow

        public void MoveToState(SubjectId stateId)
        {
            SetCurrentState(FindState(stateId));
        }


        public void AppendState(SubjectId stateId)
        {
            CurrentState.Pause();
            GameState state = FindState(stateId);
            state.gameObject.SetActive(true);
            statesStack.Add(FindState(stateId));
            CurrentState.Initialize();
        }


        public void RemoveActiveState()
        {
            statesStack.RemoveAt(statesStack.Count - 1);
            CurrentState.Resume();
        }


        #endregion



        #region Private methods


        GameState FindState(SubjectId stateId)
        {
            return statesList.FirstOrDefault((x) => x.Id == stateId);
        }


        void SetCurrentState(GameState state)
        {
            for (int i = 0; i < statesStack.Count; i++)
            {
                statesStack[i].End();
            }
            statesStack.Clear();
            Debug.Log("GameFlow : Set current stage : " + state.Id.name);
            for(int i = 0; i < statesList.Count; i++)
            {
                statesList[i].gameObject.SetActive(statesList[i] == state);
            }
            statesStack.Add(state);
        }

        #endregion
    }
}