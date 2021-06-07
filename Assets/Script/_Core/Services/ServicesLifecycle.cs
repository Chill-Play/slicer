using UnityEngine;
using System.Collections;

namespace GameFramework.Core
{
    public class ServicesLifecycle
    {
        public class OnRegistred : ILifecycleEventType { }
        public class OnRemoved: ILifecycleEventType { }
    }
}