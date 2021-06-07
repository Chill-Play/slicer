using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Core
{
    public class UnityLifecycleListener : MonoBehaviour
    {

        // Use this for initialization
        void OnEnable() {

        }


        private void OnDisable()
        {

        }



        // Update is called once per frame
        void Update() 
        {
            ObjectsLifecycleSystem.Instance.CallGlobal<PreUpdateEvent>();
            ObjectsLifecycleSystem.Instance.CallGlobal<UpdateEvent>();
        }


        void LateUpdate()
        {
            ObjectsLifecycleSystem.Instance.CallGlobal<LateUpdateEvent>();
        }


        void FixedUpdate()
        {
            ObjectsLifecycleSystem.Instance.CallGlobal<FixedUpdateEvent>();
        }

        public class PreUpdateEvent : ILifecycleEventType { }

        public class UpdateEvent : ILifecycleEventType { }

        public class FixedUpdateEvent : ILifecycleEventType { }

        public class LateUpdateEvent : ILifecycleEventType { }
    }
}