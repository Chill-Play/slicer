using UnityEngine;


namespace GameFramework.Core
{
    public class UnityLogger : Singleton<UnityLogger> //Move to DI
    {
        #region Variables

        [SerializeField] LogLevel logLevel;

        #endregion



        #region ILogger

        public void Log(LogLevel logLevel, string message)
        {
            if(ShouldLog(logLevel))
            {
                Debug.Log(message);
            }
        }

        public void LogError(LogLevel logLevel, string message)
        {
            if (ShouldLog(logLevel))
            {
                Debug.LogError(message);
            }
        }

        public void LogWarning(LogLevel logLevel, string message)
        {
            if (ShouldLog(logLevel))
            {
                Debug.LogWarning(message);
            }
        }

        #endregion



        #region Private methods

        bool ShouldLog(LogLevel level)
        {
            return logLevel >= level;
        }

        #endregion
    }
}