using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameFramework.Core
{
    public sealed class WorldNode
    {
        #region Variables

        WorldNode parent;

        #endregion



        #region Properties

        public WorldNode Root
        {
            get
            {
                WorldNode result = null;
                WorldNode temp = this;
                while (temp != null)
                {
                    result = temp;
                    temp = temp.parent;
                }
                return result;
            }
        }

        public bool IsRoot
        {
            get
            {
                return parent == null;
            }
        }


        public IObject ConnectedObject { get; set; }

        public List<WorldNode> Childs { get; } = new List<WorldNode>();

        #endregion



        #region Constructors

        public WorldNode(IObject connectedObject)
        {
            ConnectedObject = connectedObject;
        }

        #endregion



        #region Public methods

        public bool IsChildOf(WorldNode node)
        {
            WorldNode tempNode = this;
            while(tempNode.parent != null)
            {
                if(tempNode.parent == node)
                {
                    return true;
                }
                tempNode = tempNode.parent;
            }
            return false;
        }

        public void SetParent(WorldNode node)
        {
            if(parent != null)
            {
                parent.RemoveChild(node);
            }
            parent = node;
            if (parent != null)
            {
                parent.AddChild(this);
            }
        }


        public void Destroy()
        {
            parent.RemoveChild(this);
        }

        #endregion



        #region Private methods

        void AddChild(WorldNode node)
        {
            Childs.Add(node);
        }


        void RemoveChild(WorldNode node)
        {
            Childs.Remove(node);
        }

        #endregion
    }
}