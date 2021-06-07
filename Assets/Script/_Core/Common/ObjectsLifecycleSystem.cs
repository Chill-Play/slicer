using UnityEngine;
using System.Collections.Generic;
using System;

namespace GameFramework.Core
{
    public class ObjectsLifecycleSystem : Singleton<ObjectsLifecycleSystem>
    {
        #region Variables

        Dictionary<System.Type, ILifecycleEventSender> senders = new Dictionary<System.Type, ILifecycleEventSender>();

        #endregion



        #region Public methods

        public void RegisterSender(ILifecycleEventSender sender)
        {
            senders.Add(sender.GetType().GenericTypeArguments[0], sender);
        }


        public void RemoveSender(ILifecycleEventSender sender)
        {
            senders.Remove(sender.GetType().GenericTypeArguments[0]);
        }


        public void BindLifecycle(IObject coreObject)
        {
            Dictionary<System.Type, ILifecycleBinding> bindingInfo = LifecycleBinderUtil.GetLifecycleBindingsForObject(coreObject);
            foreach (KeyValuePair<System.Type, ILifecycleBinding> entry in bindingInfo)
            {
                if (senders.ContainsKey(entry.Key))
                {
                    senders[entry.Key].AddBinding(entry.Value);
                }
                else
                {
                    CreateSender(entry.Key, entry.Value.GetType().GenericTypeArguments);
                    senders[entry.Key].AddBinding(entry.Value);
                }
            }
        }


        public void CallGlobal<T>()
        {
            if (senders.TryGetValue(typeof(T), out ILifecycleEventSender sender))
            {
                ((LifecycleEventSender<T>)sender).CallGlobal();
            }
        }


        public void CallLocal<T>(IObject target)
        {
            if (senders.TryGetValue(typeof(T), out ILifecycleEventSender sender))
            {
                ((LifecycleEventSender<T>)sender).Call(target);
            }
        }


        public void CallGlobal<T, T0>(T0 param)
        {
            if(senders.TryGetValue(typeof(T), out ILifecycleEventSender sender))
            {
                ((LifecycleEventSender<T, T0>)sender).CallGlobal(param);
            }
        }


        public void CallLocal<T, T0>(IObject target, T0 param)
        {
            if (senders.TryGetValue(typeof(T), out ILifecycleEventSender sender))
            {
                ((LifecycleEventSender<T, T0>)sender).Call(target, param);
            }
        }


        public void UnbindLifecycle(IObject coreObject)
        {
            foreach (KeyValuePair<System.Type, ILifecycleEventSender> entry in senders)
            {
                if (senders.ContainsKey(entry.Key))
                {
                    senders[entry.Key].RemoveBinding(coreObject);
                }
            }
        }

        #endregion


        #region Private methods


        private void CreateSender(Type callType, params Type[] argTypes)
        {
            Type senderType;
            if(argTypes.Length == 0)
            {
                senderType = typeof(LifecycleEventSender<>).MakeGenericType(callType);
            }
            else
            {
                Type[] types = new Type[argTypes.Length + 1];
                types[0] = callType;
                for(int i = 0; i < argTypes.Length; i++)
                {
                    types[i + 1] = argTypes[i];
                }
                senderType = typeof(LifecycleEventSender<>).MakeGenericType(types);
            }
            ILifecycleEventSender sender = System.Activator.CreateInstance(senderType) as ILifecycleEventSender;
            senders.Add(callType, sender);
        }

        #endregion
    }
}