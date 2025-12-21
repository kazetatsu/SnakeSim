using UnityEngine;
#if UNITY_EDITOR
using System;
using System.IO;

[System.Serializable]
#endif
public class Pose {
    public Quaternion[] rotations;
}


public static class PoseIO {
    public static Pose Read(string fileName) {
        var t = Resources.Load<TextAsset>(fileName.Split('.')[0]);
        if (t is not null)
            return JsonUtility.FromJson<Pose>(t.text);

        #if UNITY_EDITOR
        string path = Path.Combine(Application.dataPath, fileName);
        if (File.Exists(path)) {
            var reader = new StreamReader(path);
            var pose = JsonUtility.FromJson<Pose>(reader.ReadLine());
            reader.Close();
            return pose;
        } else {
            return null;
        }
        #else
        return null;
        #endif
    }

    #if UNITY_EDITOR
    public static void Write(Pose pose, string fileName) {
        string path = Path.Combine(Application.dataPath, fileName);
        var writer = new StreamWriter(path, false);
        writer.WriteLine(JsonUtility.ToJson(pose));
        writer.Close();
    }
    #endif
}
