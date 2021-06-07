using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;


namespace GameFramework.Core
{
    public class GameManager : MonoBehaviour
    {
        #region Events

        #endregion



        #region Unity lifecycle

        void Awake()
        {
            GenerateContext();
            Application.targetFrameRate = 60;                       
        }


        void OnDisable()
        {
           
        }

        #endregion



        #region Private methods

        void GenerateContext()
        {
            ScriptableObject[] scriptableObjects = Resources.LoadAll<ScriptableObject>(string.Empty);
            for(int i = 0; i < scriptableObjects.Length; i++)
            {
                IContextInstaller installer = scriptableObjects[i] as IContextInstaller;
                if(installer != null)
                {
                    installer.Install();
                }
            }
        }

        #endregion
    }
}