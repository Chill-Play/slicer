using UnityEngine;
using System.Collections.Generic;


namespace GameFramework.Core
{
    public class World : Singleton<World>
    {
        #region Variables

        Dictionary<IObject, WorldNode> nodes = new Dictionary<IObject, WorldNode>();
        WorldPlane plane = new WorldPlane(Vector3.zero, Quaternion.identity, Vector3.one * 500); //TODO: Create plane dynamicly

        #endregion



        #region Properties

        public WorldNode RootNode { get; } = new WorldNode(null);
        public WorldPlane Plane => plane;

        #endregion



        #region Public methods

        public void AddObject(IObject obj)
        {
            AddObject(obj, null);
        }


        public void AddObject(IObject obj, IObject parent)
        {
            WorldNode node = new WorldNode(obj);

            if (parent != null)
            {
                node.SetParent(nodes[parent]);
            }
            else
            {
                node.SetParent(RootNode);
            }

            nodes.Add(obj, node);
        }


        public void RemoveObject(IObject obj)
        {
            nodes[obj].Destroy();
            nodes.Remove(obj);
        }


        public WorldNode GetNode(IObject obj)
        {
            return nodes[obj];
        }

        #endregion
    }
}