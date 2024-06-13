using DG.Tweening.Plugins.Core.PathCore;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void Save(GameData data)
    {
        string path = Application.persistentDataPath + "/data.qnd";
        BinaryFormatter formatter = new();
        FileStream fs = new(path, FileMode.Create);
        formatter.Serialize(fs, data);
        fs.Close();
    }

    public static GameData Load()
    {
        if (!File.Exists(GetPath()))
        {
            GameData emptyData = new();
            Save(emptyData);
            return emptyData;
        }

        BinaryFormatter formatter = new();
        FileStream fs = new(GetPath(), FileMode.Open);
        GameData data = formatter.Deserialize(fs) as GameData;
        fs.Close();

        return data;
    }

    private static string GetPath()
    {
        return Application.persistentDataPath + "/data.qnd";
    }
}
