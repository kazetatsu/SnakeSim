using System.IO;
using UnityEngine;

[System.Serializable]
public class Pose {
    public Quaternion[] rotations;
}


public static class PoseIO {
    public static Pose Read(string fileName) {
        string path = Path.Combine(Application.dataPath, fileName);
        if (File.Exists(path)) {
            var reader = new StreamReader(path);
            var pose = JsonUtility.FromJson<Pose>(reader.ReadLine());
            reader.Close();
            return pose;
        } else {
            return null;
        }
    }


    public static void Write(Pose pose, string fileName) {
        string path = Path.Combine(Application.dataPath, fileName);
        var writer = new StreamWriter(path, false);
        writer.WriteLine(JsonUtility.ToJson(pose));
        writer.Close();
    }
}
