using UnityEngine;
using System.Collections;
using System;

namespace GameFramework.Core
{
    public interface IEventReceiver
    {
        int EventId { get; }
        IObject Owner { get; }
    }


    public interface IEventReceiver<T> : IEventReceiver
    {
        void Subscribe(IObject owner, Action<T> _callback);
        void Unsubscribe();
        void FireEvent(T _data);
    }


    public interface IEventReceiverSimple : IEventReceiver
    {
        void Subscribe(IObject owner, Action _callback);
        void Unsubscribe();
        void FireEvent();
    }
}