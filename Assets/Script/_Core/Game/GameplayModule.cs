using UnityEngine;


namespace GameFramework.Core
{
    [CreateAssetMenu(fileName = "gameplay_module_", menuName = "Game/Settings/Module")]
    public class GameplayModule : IObject
    {
        #region variables

        [SerializeField] protected bool disabled;

        bool active;

        #endregion



        #region Properties

        public bool Active
        {
            get
            {
                return active && !disabled;
            }
        }

        #endregion



        #region IObject

        public bool Enabled
        {
            get
            {
                return !disabled;
            }

            set
            {
                disabled = !value;
            }
        }

        #endregion



        #region Public methods


        public void SetActive(bool active)
        {
            this.active = active;
        }


        public virtual void Initialize()
        {
            active = true;
        }


        public virtual void End()
        {
            active = false;
        }


        public virtual void DrawDebugInfo()
        {

        }

        #endregion
    }
}