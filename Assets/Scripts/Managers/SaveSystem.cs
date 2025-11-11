using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/playerdata.json";

    public static void SaveData(PlayerData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("✅ SaveSystem: Data saved to " + savePath);
    }

    public static PlayerData LoadData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("✅ SaveSystem: Data loaded from " + savePath);
            return data;
        }

        Debug.LogWarning("⚠️ SaveSystem: No save file found!");
        return null;
    }

    public static void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("✅ SaveSystem: Save file deleted.");
        }
    }

    public static bool HasSave()
    {
        return File.Exists(savePath);
    }
}
