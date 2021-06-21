using UnityEngine;

namespace GameFramework.Core
{
    public class GameState : MonoBehaviour
    {
        #region Variables

        [SerializeField] SubjectId id;

        #endregion


        #region Properties

        public SubjectId Id => id;

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