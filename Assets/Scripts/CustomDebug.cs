using UnityEngine;

public class CustomDebug : MonoBehaviour
{
    public bool enableDebug = true;
    public static void Log(string message)
    {
        if (Instance.enableDebug)
        {
            string callingScript = StackTraceUtility.ExtractStackTrace().Split('\n')[2].Trim();
            string logMessage = $"{callingScript} - {message}";
            Debug.Log(logMessage);
        }
    }

    public static void LogWarning(string message)
    {
        if (Instance.enableDebug)
        {
            string callingScript = StackTraceUtility.ExtractStackTrace().Split('\n')[2].Trim();
            string logMessage = $"{callingScript} - {message}";
            Debug.LogWarning(logMessage);
        }
    }

    public static void LogEarror(string message)
    {
        if (Instance.enableDebug)
        {
            string callingScript = StackTraceUtility.ExtractStackTrace().Split('\n')[2].Trim();
            string logMessage = $"{callingScript} - {message}";
            Debug.LogError(logMessage);
        }
    }
    private static CustomDebug _instance;
    public static CustomDebug Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CustomDebug>();
                if (_instance == null)
                {
                    Debug.LogError("CustomDebug instance not found in the scene.");
                }
            }

            return _instance;
        }
    }
}