using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData {
    public int spawnPointID;

    public SaveData() {
        spawnPointID = 0;
    }
}

public class SaverLoader {
    string filepath = Application.dataPath + "/savedata.json";
    public SaveData data;


    public SaverLoader() { }


    public void Load() {
        if (File.Exists(filepath)) {
            var reader = new StreamReader(filepath);
            data = JsonUtility.FromJson<SaveData>(reader.ReadLine());
            reader.Close();
        } else
            data = new SaveData();
    }


    public void Save() {
        var writer = new StreamWriter(filepath, false);
        writer.WriteLine(JsonUtility.ToJson(data));
        writer.Close();
    }
}
