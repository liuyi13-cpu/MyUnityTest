using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 本地存贮
/// 基础类型 string int float
/// </summary>
public static class PlayerPrefsManager
{
    const string KEY_USERNAME = "KEY_USERNAME";
    const string KEY_PASSWORD = "KEY_PASSWORD";

    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public static string GetString(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static int GetInt(string key)
    {
        return PlayerPrefs.GetInt(key);
    }

    public static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public static float GetFloat(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }
}
