using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameFramework.Core
{
    public delegate void LifeсycleCall();
    public delegate void LifeсycleCall<T>(T param);
    //public delegate void LifeсycleCall<T>();

    public static class LifecycleBinderUtil
    {
        #region Variables

        static Dictionary<System.Type, ILifecycleBinding> cachedBindingDict = new Dictionary<System.Type, ILifecycleBinding>();

        #endregion


        #region Public methods

        public static Dictionary<System.Type, ILifecycleBinding> GetLifecycleBindingsForObject(IObject coreObject)
        {
            cachedBindingDict.Clear();

            Type type = coreObject.GetType();

            MethodInfo[] methods = type.GetMethods();
            for (int i = 0; i < methods.Length; i++)
            {
                LifecycleEventAttribute attribute = methods[i].GetCustomAttribute<LifecycleEventAttribute>();
                if (attribute != null)
                {
                    Type[] parametersTypes = GetParametersTypes(methods[i]);
                    ILifecycleBinding binding = null;
                    if (parametersTypes.Length == 0)
                    {
                        binding = System.Activator.CreateInstance<LifecycleBinding>();
                    }
                    else
                    {
                        binding = System.Activator.CreateInstance(typeof(LifecycleBinding<>).MakeGenericType(parametersTypes)) as ILifecycleBinding;
                    }
                    binding.Setup(methods[i], coreObject);
                    cachedBindingDict.Add(attribute.EventType, binding);
                }
            }
            return cachedBindingDict;
        }

        #endregion


        #region Private methods

        static Type[] GetParametersTypes(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            Type[] types = new Type[parameters.Length];
            for (int i = 0; i < types.Length; i++)
            {
                types[i] = parameters[i].ParameterType;
            }
            return types;
        }

        #endregion
    }


    public interface ILifecycleBinding
    {
        IObject Target { get; }
        void Setup(MethodInfo method, IObject target);
    }


    public class LifecycleBinding : ILifecycleBinding
    {
        LifeсycleCall call;

        public IObject Target { get; private set; }

        public void Setup(MethodInfo method, IObject target)
        {
            call = (LifeсycleCall)method.CreateDelegate(typeof(LifeсycleCall), target);
            Target = target;
        }

        public void Call()
        {
            if (Target.Enabled)
            {
                call.Invoke();
            }
        }
    }


    public class LifecycleBinding<T> : ILifecycleBinding
    {
        LifeсycleCall<T> call;
        public IObject Target { get; private set; }

        public void Setup(MethodInfo method, IObject target)
        {
            call = (LifeсycleCall<T>)method.CreateDelegate(typeof(LifeсycleCall<>).MakeGenericType(typeof(T)), target);
            Target = target;
        }


        public void Call(T param)
        {
            if (Target.Enabled)
            {
                call.Invoke(param);
            }
        }
    }
}