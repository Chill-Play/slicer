using UnityEngine;
using System.Reflection;
using System;
using System.Linq;


namespace GameFramework.Core
{
    public static class ReflectionUtil
    {
        #region Public methods

        public static MethodInfo[] GetMethodsWithAttribute<T>(IObject coreObject) where T : Attribute
        {
            return GetMethodsWithAttribute<T>(coreObject.GetType());
        }


        public static FieldInfo[] GetFieldsWithAttribute<T>(IObject coreObject) where T : Attribute
        {
            return GetFieldsWithAttribute<T>(coreObject.GetType());
        }


        public static MethodInfo[] GetMethodsWithAttribute<T>(Type type) where T : Attribute
        {
            MethodInfo[] methods = type.GetMethods()
                          .Where(m => m.GetCustomAttributes(typeof(T), false).Length > 0)
                          .ToArray();
            return methods;
        }


        public static FieldInfo[] GetFieldsWithAttribute<T>(Type type) where T : Attribute
        {
            FieldInfo[] fields = type.GetFields()
                          .Where(m => m.GetCustomAttributes(typeof(T), false).Length > 0)
                          .ToArray();
            return fields;
        }


        public static Type[] GetTypesWithAttribute<T>() where T : Attribute
        {
            Type[] types = (from a in AppDomain.CurrentDomain.GetAssemblies()
                           from t in a.GetTypes()
                           let attributes = t.GetCustomAttributes(typeof(T), true)
                           where attributes != null && attributes.Length > 0
                           select t).ToArray();
            return types;
        }

        #endregion
    }
}