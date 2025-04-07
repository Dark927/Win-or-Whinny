using UnityEngine;

namespace Game.Utilities.Logging
{
    public static class CustomLogger
    {
        [System.Diagnostics.Conditional("ENABLE_LOG")]
        public static void Log(string message)
        {
            Debug.Log(message);
        }

        [System.Diagnostics.Conditional("ENABLE_LOG")]
        public static void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }

        [System.Diagnostics.Conditional("ENABLE_LOG")]
        public static void LogError(string message)
        {
            Debug.LogError(message);
        }

        [System.Diagnostics.Conditional("ENABLE_LOG")]
        public static void LogComponentIsNull(string ownerName, string componentName)
        {
            Debug.LogError($"<color=red> # Error :</color> {ownerName} has {componentName} component == null!");
        }

    }
}
