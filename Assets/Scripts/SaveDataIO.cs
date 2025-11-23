using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData {
    public int spawnPointID;

    public SaveData() {
        spawnPointID = 0;
    }
}


public static class SaveDataIO {
    public static string fileName = "SaveData.json";


    public static string GetPath() {
        return Path.Combine(Application.dataPath, fileName);
    }


    public static SaveData Read() {
        string path = GetPath();
        if (File.Exists(path)) {
            var reader = new StreamReader(path);
            var data = JsonUtility.FromJson<SaveData>(reader.ReadLine());
            reader.Close();
            return data;
        } else {
            return null;
        }
    }


    public static void Write(SaveData data) {
        string path = GetPath();
        var writer = new StreamWriter(path, false);
        writer.WriteLine(JsonUtility.ToJson(data));
        writer.Close();
    }
}
