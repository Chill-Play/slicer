using UnityEngine;
using System;
using UnityEngine.Scripting;

namespace GameFramework.Core
{
    [System.AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class LifecycleEventAttribute : PreserveAttribute
    {
        public System.Type EventType { get; set; }

        public LifecycleEventAttribute(System.Type type)
        {
            if (type.GetInterface(typeof(ILifecycleEventType).Name) == null)
            {
                throw new ArgumentException("Wrong type argument, type must be LifecycleEvent");
            }
            EventType = type;
        }
    }
}