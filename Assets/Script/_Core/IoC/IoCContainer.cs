using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


namespace GameFramework.Core
{
    public class IoCContainer
    {
        #region Variables

        static IoCContainer instance;
        public Dictionary<System.Type, object> container = new Dictionary<System.Type, object>();

        #endregion



        #region Properties

        static IoCContainer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IoCContainer();
                }
                return instance;
            }
        }

        #endregion



        #region Public methods

        public static void Bind<T>(T obj)
        {
            System.Type type = typeof(T);
            if (Instance.container.ContainsKey(type))
            {
                Instance.container[type] = obj;
            }
            else
            {
                Instance.container.Add(type, obj);
            }
        }


        public static void Bind(System.Type type, object obj)
        {
            if (Instance.container.ContainsKey(type))
            {
                Instance.container[type] = obj;
            }
            else
            {
                Instance.container.Add(type, obj);
            }
        }


        public static void Unbind<T>(T obj)
        {
            System.Type type = typeof(T);
            Instance.container.Remove(type);
        }


        public static T Get<T>()
        {
            System.Type type = typeof(T);
            T result = default(T);
            if (Instance.container.ContainsKey(type))
            {
                result = (T)Instance.container[type];
            }
            return result;
        }

        #endregion
    }
}