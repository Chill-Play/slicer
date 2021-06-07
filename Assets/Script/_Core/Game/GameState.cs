using UnityEngine;

namespace GameFramework.Core
{
    public class GameState
    {
        #region Variables

        public string id;

        #endregion


        #region Public methods

        public virtual void Initialize()
        {

        }


        public void Pause()
        {

        }


        public void Resume()
        {

        }


        public virtual void End()
        {

        }

        #endregion
    }
}