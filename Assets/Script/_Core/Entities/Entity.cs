using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameFramework.Core
{
    public abstract class Entity : MonoBehaviour, IObject
    {
        #region Variables

        bool registred;

        #endregion


        #region Properties

        public int Id 
        { 
            get
            {
                return GetInstanceID();
            }
        }

        #endregion



        #region IObject

        public bool Enabled { get; set; }

        #endregion



        #region Unity lifecycle

        protected virtual void Awake()
        {
            if (!registred)
            {
                World.Instance.AddObject(this);
                RegisterEntity();
                registred = true;
            }
        }


        protected virtual void OnEnable()
        {
            if (!registred)
            {
                RegisterEntity();
                registred = true;
            }
        }


        protected virtual void OnDisable()
        {
            if (registred)
            {
                RemoveEntity();
                registred = false;
            }
        }


        protected virtual void OnDestroy()
        {
            if (registred)
            {
                World.Instance.RemoveObject(this);
                RemoveEntity();
                registred = false;
            }
        }


        #endregion



        #region Private methods

        protected abstract void RegisterEntity();


        protected abstract void RemoveEntity();

        #endregion

    }



    public class Entity<T> : Entity where T : Entity<T>
    {

        #region Private methods

        protected override void RegisterEntity()
        {
            IoCContainer.Get<EntityService>().AddEntity((T)this);
        }


        protected override void RemoveEntity()
        {
            IoCContainer.Get<EntityService>().RemoveEntity(this);
        }

        #endregion
    }
}