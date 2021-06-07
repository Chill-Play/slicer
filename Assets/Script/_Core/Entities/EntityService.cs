using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace GameFramework.Core
{
    [AutoInitializeService]
    public class EntityService : IService
    {
        #region Variables

        Dictionary<System.Type, EntitiesContainer> containers = new Dictionary<System.Type, EntitiesContainer>();

        #endregion



        #region IService

        public bool Enabled { get; set; }

        #endregion



        #region IEntitySystem


        public T[] GetEntities<T>() where T : Entity
        {
            EntitiesContainer<T> container = GetContainer<T>();

            return container.Entities;
        }


        public T GetFirstEntity<T>() where T : Entity
        {
            T[] entities = GetEntities<T>();
            return entities[0];
        }


        public int GetEntitiesCount<T>() where T : Entity
        {
            EntitiesContainer<T> container = GetContainer<T>();
            return container.Count;
        }


        public void AddEntity<T>(T entity) where T : Entity
        {
            EntitiesContainer<T> container = GetContainer<T>();
            container.AddEntity(entity);
        }


        public void RemoveEntity(Entity entity)
        {
            System.Type type = entity.GetType();
            if (containers.ContainsKey(type))
            {
                containers[type].RemoveEntity(entity);
            }
            else
            {
                if (!Application.isLoadingLevel)
                {
                    UnityLogger.Instance.LogError(LogLevel.Critical, "No containers created for : " + entity.GetType());
                }
            }
        }

        public EntitiesContainer<T> GetContainer<T>() where T : Entity
        {
            System.Type type = typeof(T);
            if (!containers.ContainsKey(type))
            {
                containers.Add(type, new EntitiesContainer<T>());
            }
            return containers[type] as EntitiesContainer<T>;
        }


        public EntitiesContainer GetContainer(Type type)
        {
            if (!containers.ContainsKey(type))
            {
                Type containerType = typeof(EntitiesContainer<>).MakeGenericType(type);
                containers.Add(type, System.Activator.CreateInstance(containerType) as EntitiesContainer);
            }
            return containers[type];
        }


        #endregion

    }
}
