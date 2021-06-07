//using UnityEngine;
//using System.Collections.Generic;


//namespace GameFramework.Core
//{
//    public class LifecycleEventContainer
//    {
//        #region Variables

//        Dictionary<IObject, ILifecycleBinding> bindings = new Dictionary<IObject, ILifecycleBinding>();

//        #endregion



//        #region Properties

//        public int Id
//        {
//            get
//            {
//                return GetHashCode();
//            }
//        }

//        #endregion



//        #region Public methods

//        public void AddBinding(IObject instance, ILifecycleBinding binding)
//        {
//            bindings.Add(instance, binding);
//        }


//        public void RemoveBinding(IObject instance)
//        {
//            bindings.Remove(instance);
//        }


//        public void Fire()
//        {
//            foreach (KeyValuePair<IObject, ILifecycleBinding> entry in bindings)
//            {
//                entry.Value.Call();
//            }
//        }


//        public void Fire(IObject target)
//        {
//            if (bindings.ContainsKey(target))
//            {
//                bindings[target].Call();
//            }
//        }


//        public void Fire(IObject[] objects)
//        {
//            for(int i = 0; i < objects.Length; i++)
//            {
//                if (bindings.ContainsKey(objects[i]))
//                {
//                    bindings[objects[i]].Call();
//                }
//            }
//        }

//        #endregion
//    }
//}