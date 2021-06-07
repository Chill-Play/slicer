using GameFramework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AutoInitializeService()]
public class WorldEntityQueryService : IService
{
    Dictionary<Type, WorldEntityQuadTree> trees = new Dictionary<Type, WorldEntityQuadTree>();

    #region IService

    public bool Enabled { get; set; }

    #endregion

    public IEnumerable<T> GetEntitiesInExtents<T>(Vector3 point, float distance) where T: Entity
    {
        WorldEntityQuadTree tree;
        if(trees.TryGetValue(typeof(T), out WorldEntityQuadTree qt))
        {
            tree = qt;
        }
        else
        {
            tree = CreateQuadTree<T>();
        }

        EntitiesContainer<T> container = IoCContainer.Get<EntityService>().GetContainer<T>();
        IntList queryList = tree.Query(tree.GetElementRect(point, distance), -1);
        for(int i = 0; i < queryList.Count; i++)
        {
            yield return container.Entities[queryList.Get(i, 0)];
        }
    }


    private WorldEntityQuadTree CreateQuadTree<T>() where T : Entity
    {
        WorldEntityQuadTree tree;
        tree = new WorldEntityQuadTree<T>(); //TODO: Elements count to some sort of settings
        tree.Build();
        trees.Add(typeof(T), tree);
        return tree;
    }


    [LifecycleEvent(typeof(UnityLifecycleListener.PreUpdateEvent))]
    public void PreUpdate()
    {
        foreach(var tree in trees.Values)
        {
            tree.Update();
        }
    }
}
