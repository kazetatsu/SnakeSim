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
    public static SaveData CreateOrRead(string fileName) {
        string path = Path.Combine(Application.dataPath, fileName);
        if (File.Exists(path)) {
            var reader = new StreamReader(path);
            var data = JsonUtility.FromJson<SaveData>(reader.ReadLine());
            reader.Close();
            return data;
        } else {
            return new SaveData();
        }
    }


    public static void Write(SaveData data, string fileName) {
        string path = Path.Combine(Application.dataPath, fileName);
        var writer = new StreamWriter(path, false);
        writer.WriteLine(JsonUtility.ToJson(data));
        writer.Close();
    }
}
