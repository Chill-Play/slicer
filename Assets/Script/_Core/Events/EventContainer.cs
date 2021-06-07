using System;
using System.Collections.Generic;


namespace GameFramework.Core
{
    public abstract class EventContainer
    {
        #region Properties

        public abstract Type DataType { get; }
        public bool CacheData { get; set; }
        public DataId DataId { get; set; }

        #endregion
    }


    public class EventContainer<T> : EventContainer
    {
        #region Variables

        Dictionary<IObject, IEventReceiver<T>> receiversPerOwner = new Dictionary<IObject, IEventReceiver<T>>();
        HashSet<IEventReceiver<T>> receivers = new HashSet<IEventReceiver<T>>();
        HashSet<IEventReceiver<T>> addedReceivers = new HashSet<IEventReceiver<T>>();
        HashSet<IEventReceiver<T>> removedReceivers = new HashSet<IEventReceiver<T>>();

        bool lockedReceiversModifications;

        #endregion



        #region Properties

        public override Type DataType => typeof(T);

        #endregion



        #region Public Methods

        public void AddEventReceiver(IEventReceiver<T> _receiver)
        {
            receiversPerOwner.Add(_receiver.Owner, _receiver);
            if (!receivers.Contains(_receiver))
            {
                if (!lockedReceiversModifications)
                {
                    receivers.Add(_receiver);
                }
                else
                {
                    addedReceivers.Add(_receiver);
                }
            }
        }


        public void RemoveEventReceiver(IEventReceiver<T> _receiver)
        {
            if (receivers.Contains(_receiver))
            {
                if (!lockedReceiversModifications)
                {
                    receivers.Remove(_receiver);
                }
                else
                {
                    removedReceivers.Add(_receiver);
                }
            }
        }


        public void SendEvent(T _data)
        {
            if(CacheData)
            {
                DataVault.Instance.Push<T>(_data, DataId);
            }
            lockedReceiversModifications = true;

            foreach (IEventReceiver<T> receiver in receivers)
            {
                receiver.FireEvent(_data);
            }

            lockedReceiversModifications = false;

            if (addedReceivers.Count > 0)
            {
                receivers.UnionWith(addedReceivers);
                addedReceivers.Clear();
            }

            if (removedReceivers.Count > 0)
            {
                receivers.ExceptWith(removedReceivers);
                removedReceivers.Clear();
            }
        }


        public void SendEvent(IObject target, T _data)
        {
            if (CacheData)
            {
                DataVault.Instance.Push<T>(_data, DataId);
            }
            lockedReceiversModifications = true;

            //WorldNode parentNode = World.Instance.GetNode(target);
            //foreach (IEventReceiver<T> receiver in receivers)
            //{
            //    if(receiver.Owner != null)
            //    {
            //        WorldNode node = World.Instance.GetNode(receiver.Owner);
            //        if(node == parentNode || node.IsChildOf(parentNode))
            //        {
            //            receiver.FireEvent(_data);
            //        }
            //    }
            //}


            WorldNode currentNode = World.Instance.GetNode(target);
            SendEventTree(currentNode, _data);

            lockedReceiversModifications = false;

            if (addedReceivers.Count > 0)
            {
                receivers.UnionWith(addedReceivers);
                addedReceivers.Clear();
            }

            if (removedReceivers.Count > 0)
            {
                receivers.ExceptWith(removedReceivers);
                removedReceivers.Clear();
            }
        }



        void SendEventTree(WorldNode currentNode, T _data)
        {
            for (int i = 0; i < currentNode.Childs.Count; i++)
            {
                if (receiversPerOwner.ContainsKey(currentNode.ConnectedObject))
                {
                    IEventReceiver<T> receiver = receiversPerOwner[currentNode.ConnectedObject];
                    if (receiver != null)
                    {
                        receiver.FireEvent(_data);
                    }
                }
                SendEventTree(currentNode.Childs[i], _data);
            }

        }
        #endregion
    }



    public class EventContainerSimple : EventContainer
    {
        #region Variables

        Dictionary<IObject, IEventReceiverSimple> receiversPerOwner = new Dictionary<IObject, IEventReceiverSimple>();
        HashSet<IEventReceiverSimple> receivers = new HashSet<IEventReceiverSimple>();
        HashSet<IEventReceiverSimple> addedReceivers = new HashSet<IEventReceiverSimple>();
        HashSet<IEventReceiverSimple> removedReceivers = new HashSet<IEventReceiverSimple>();

        bool lockedReceiversModifications;

        #endregion



        #region Properties

        public override Type DataType => null;

        #endregion



        #region Public Methods

        public void AddEventReceiver(IEventReceiverSimple _receiver)
        {
            receiversPerOwner.Add(_receiver.Owner, _receiver);
            if (!receivers.Contains(_receiver))
            {
                if (!lockedReceiversModifications)
                {
                    receivers.Add(_receiver);
                }
                else
                {
                    addedReceivers.Add(_receiver);
                }
            }
        }


        public void RemoveEventReceiver(IEventReceiverSimple _receiver)
        {
            if (receivers.Contains(_receiver))
            {
                if (!lockedReceiversModifications)
                {
                    receivers.Remove(_receiver);
                }
                else
                {
                    removedReceivers.Add(_receiver);
                }
            }
        }


        public void SendEvent()
        {
            lockedReceiversModifications = true;

            foreach (IEventReceiverSimple receiver in receivers)
            {
                receiver.FireEvent();
            }

            lockedReceiversModifications = false;

            if (addedReceivers.Count > 0)
            {
                receivers.UnionWith(addedReceivers);
                addedReceivers.Clear();
            }

            if (removedReceivers.Count > 0)
            {
                receivers.ExceptWith(removedReceivers);
                removedReceivers.Clear();
            }
        }


        public void SendEvent(IObject target)
        {
            lockedReceiversModifications = true;

            WorldNode parentNode = World.Instance.GetNode(target);
            SendEventTree(parentNode);

            lockedReceiversModifications = false;

            if (addedReceivers.Count > 0)
            {
                receivers.UnionWith(addedReceivers);
                addedReceivers.Clear();
            }

            if (removedReceivers.Count > 0)
            {
                receivers.ExceptWith(removedReceivers);
                removedReceivers.Clear();
            }
        }


        void SendEventTree(WorldNode currentNode)
        {
            for (int i = 0; i < currentNode.Childs.Count; i++)
            {
                if (receiversPerOwner.ContainsKey(currentNode.ConnectedObject))
                {
                    IEventReceiverSimple receiver = receiversPerOwner[currentNode.ConnectedObject];
                    if (receiver != null)
                    {
                        receiver.FireEvent();
                    }
                }
                SendEventTree(currentNode.Childs[i]);
            }

        }

        #endregion
    }

}
