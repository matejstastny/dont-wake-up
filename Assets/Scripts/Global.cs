/*
 * Author: Matěj Šťastný
 * Date created: 6/5/2025
 * GitHub link: https://github.com/matysta/dont-wake-up
 */

using System;
using UnityEngine;

public static class Global
{
    public static void Log(string message)
    {
        DateTime now = DateTime.Now;
        string timestamp = $"{now:HH:mm:ss.fff}";
        Debug.Log("[" + timestamp + "] " + message);
    }
}