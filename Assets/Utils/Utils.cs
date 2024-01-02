using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public static class Utils
{
    public static readonly string SettingsFilePath = $"{Application.persistentDataPath}/settings.json";

    public static EditorSettings GetEditorSettings()
    {
        var json = File.ReadAllText(SettingsFilePath);
        return JsonUtility.FromJson<EditorSettings>(json);
    }

    public static void GetGameSettings()
    {
        throw new NotImplementedException();
        //var json = File.ReadAllText(SettingsFilePath);
        //return JsonUtility.FromJson<EditorSettings>(json);
    }

    public static HiddenGameSettings GetHiddenGameSettings()
    {
        var json = File.ReadAllText(SettingsFilePath);
        return JsonUtility.FromJson<EditorSettings>(json).HiddenGameSettings;
    }

    public static void SaveEditorSettings(EditorSettings settings)
    {
        var json = JsonUtility.ToJson(settings);
        File.WriteAllText(SettingsFilePath, json);
    }
}
