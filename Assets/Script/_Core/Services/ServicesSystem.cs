using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AutoInitializeServiceAttribute : Attribute
{

}


namespace GameFramework.Core
{
    public class ServicesSystem : Singleton<ServicesSystem>
    {
        #region Variables

        Dictionary<System.Type, IService> services = new Dictionary<System.Type, IService> ();

        #endregion


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            Type[] types = ReflectionUtil.GetTypesWithAttribute<AutoInitializeServiceAttribute>();
            for(int i = 0; i < types.Length; i++)
            {
                IService service = (IService)System.Activator.CreateInstance(types[i]);
                Instance.services.Add(service.GetType(), service);
                IoCContainer.Bind(types[i], service);
                service.Enabled = true;
                World.Instance.AddObject(service);
                ObjectsLifecycleSystem.Instance.BindLifecycle(service);
                Debug.Log("Service Added : " + types[i]);
            }
        }



        #region Public methods

        //public void RegisterService<T>(T service) where T : IService
        //{
        //    services.Add(service.GetType(), service);
        //    IoCContainer.Bind(service);
        //    service.Enabled = true;
        //    World.Instance.AddObject(service);
        //    ObjectsLifecycleSystem.Instance.BindLifecycle(service);
        //}


        //public void RemoveService(IService service)
        //{
        //    services.Remove(service.GetType());
        //    IoCContainer.Unbind(service);
        //    service.Enabled = false;
        //    World.Instance.RemoveObject(service);
        //    ObjectsLifecycleSystem.Instance.UnbindLifecycle(service);
        //}

        #endregion
    }
}