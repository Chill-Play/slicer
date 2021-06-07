#define CUSTOM_DEBUG

using UnityEngine;
using System.Collections;
using System.Diagnostics;


namespace GameFramework.Core
{
    public enum LogLevel
    {
        None = 0,
        ErrorsOnly = 1,
        Critical = 2,
        Verbose = 3
    }

    public abstract class Logger
    {
        [Conditional("UNITY_EDITOR"), Conditional("UNITY_DEBUG")]
        public abstract void Log(LogLevel logLevel, string message);
        [Conditional("UNITY_EDITOR"), Conditional("UNITY_DEBUG")]
        public abstract void LogError(LogLevel logLevel, string message);
        [Conditional("UNITY_EDITOR"), Conditional("UNITY_DEBUG")]
        public abstract void LogWarning(LogLevel logLevel, string message);
    }
}