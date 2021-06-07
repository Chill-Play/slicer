using UnityEngine;
using System.Collections.Generic;

namespace GameFramework.Core
{
    public interface ILifecycleEventSender
    {
        void AddBinding(ILifecycleBinding binding);
        void RemoveBinding(IObject binding);
    }


    public class LifecycleEventSender<T> : ILifecycleEventSender
    {
        List<LifecycleBinding> bindings = new List<LifecycleBinding>();
        Dictionary<IObject, LifecycleBinding> bindingsByObject = new Dictionary<IObject, LifecycleBinding>();


        public void AddBinding(ILifecycleBinding binding)
        {
            bindings.Add(binding as LifecycleBinding);
            bindingsByObject.Add(binding.Target, binding as LifecycleBinding);
        }


        public void RemoveBinding(IObject obj)
        {
            LifecycleBinding binding = bindingsByObject[obj];
            bindings.Remove(binding);
            bindingsByObject.Remove(obj);
        }


        public void CallGlobal()
        {
            for(int i = 0; i < bindings.Count; i++)
            {
                bindings[i].Call();
            }
        }


        public void Call(IObject target)
        {
            bindingsByObject[target].Call();
        }
    }


    public class LifecycleEventSender<T, T0> : ILifecycleEventSender
    {
        List<LifecycleBinding<T0>> bindings = new List<LifecycleBinding<T0>>();
        Dictionary<IObject, LifecycleBinding<T0>> bindingsByObject = new Dictionary<IObject, LifecycleBinding<T0>>();


        public void AddBinding(ILifecycleBinding binding)
        {
            bindings.Add(binding as LifecycleBinding<T0>);
            bindingsByObject.Add(binding.Target, binding as LifecycleBinding<T0>);
        }


        public void RemoveBinding(IObject obj)
        {
            LifecycleBinding<T0> binding = bindingsByObject[obj];
            bindings.Remove(binding);
            bindingsByObject.Remove(obj);
        }


        public void CallGlobal(T0 param)
        {
            for (int i = 0; i < bindings.Count; i++)
            {
                bindings[i].Call(param);
            }
        }


        public void Call(IObject target, T0 param)
        {
            bindingsByObject[target].Call(param);
        }
    }
}