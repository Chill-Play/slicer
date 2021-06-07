using GameFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldEntityQuadTree
{
    protected QuadTree quadTree;
    public abstract void Build();
    public abstract void Update();

    public IntList Query(QTElementRect rect, int omitElement = -1)
    {
        return quadTree.Query(rect, omitElement);
    }


    public QTElementRect GetElementRect(Vector3 point, float extents)
    {
        return quadTree.GetElementRect(point, extents);
    }
}


public class WorldEntityQuadTree<T> : WorldEntityQuadTree where T : Entity
{
    EntitiesContainer<T> container;

    public override void Build()
    {
        CreateQuadTree();
        container = IoCContainer.Get<EntityService>().GetContainer<T>();
    }


    public override void Update()
    {
        T[] entities = container.Entities;
        quadTree.Clear(); // TODO: Clean up, not clear
        for (int i = 0; i < entities.Length; i++)
        {
            quadTree.AddElement(entities[i].Id, quadTree.GetElementRect(entities[i].transform.position, 1f));
        }
    }



    private void CreateQuadTree()
    {
        quadTree = new QuadTree(World.Instance.Plane, 5, 8); //TODO: Elements count to some sort of settings
    }
}