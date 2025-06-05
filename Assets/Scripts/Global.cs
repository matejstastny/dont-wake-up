using System;
using UnityEngine;

public class Global
{
    public static void Log(string message)
    {
        DateTime now = DateTime.Now;
        string timestamp = $"{now:HH:mm:ss.fff}";
        Debug.Log("[" + timestamp + "] " + message);
    }
}