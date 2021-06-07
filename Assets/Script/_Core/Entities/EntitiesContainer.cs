using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Core
{
    public abstract class EntitiesContainer
    {
        #region Public methods

        public abstract void AddEntity(Entity entity);


        public abstract void RemoveEntity(Entity entity);

        #endregion
    }


    public class EntitiesContainer<T> : EntitiesContainer where T : Entity
    {

        #region Variables

        public event System.Action<T> OnEntityAdded;
        public event System.Action<T> OnEntityRemoved;

        List<T> entitiesList = new List<T>();

        #endregion



        #region Properties

        public T[] Entities { get; private set; } = new T[0];


        public int Count { get; private set; } = 0;

        #endregion



        #region Public methods

        public override void AddEntity(Entity entity)
        {
            T element = entity as T;
            OnEntityAdded?.Invoke(element);
            entitiesList.Add(element);
            Entities = entitiesList.ToArray();
            Count = Entities.Length;
        }

        public override void RemoveEntity(Entity entity)
        {
            T element = entity as T;
            OnEntityRemoved?.Invoke(element);
            entitiesList.Remove(element);
            Entities = entitiesList.ToArray();
            Count = Entities.Length;
        }

        #endregion

    }
}
