using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;


[System.Serializable]
public class PoseData {
    public Quaternion[] rots;
    public Quaternion rootRot;
    public Vector3 rootPos;
}


public class Pose : MonoBehaviour
{
    int segmentsCount;
    Transform[] segments;
    [SerializeField] float dist;
    [SerializeField] Quaternion[] rots;
    InputAction save;
    InputAction load;
    string filepath;


    void Start() {
        segmentsCount = transform.childCount;

        segments = new Transform[segmentsCount];
        for (int i = 0; i < segmentsCount; ++i) {
            segments[i] = transform.GetChild(i);
            segments[i].position = ((float)i + 0.5f) * dist * Vector3.back;
        }

        rots = new Quaternion[segmentsCount - 1];
        for (int i = 0; i < segmentsCount - 1; ++i)
            rots[i] = Quaternion.identity;

        save = InputSystem.actions.FindAction("SaveDebug");
        load = InputSystem.actions.FindAction("LoadDebug");
        filepath = Application.dataPath + "/pose.json";
        if (!File.Exists(filepath)) {
            var writer = new StreamWriter(filepath, false);
            writer.Write("");
            writer.Close();
        }
    }


    void Update() {
        for (int i = segmentsCount - 2; i >= 0; --i) {
            segments[i].rotation = rots[i] * segments[i + 1].rotation;
            segments[i].position = 0.5f * dist * (rots[i] * segments[i + 1].forward)
                                 + 0.5f * dist * segments[i + 1].forward
                                 + segments[i + 1].position;
        }

        if (save.IsPressed()) {
            var data = new PoseData();
            data.rots = rots;
            data.rootPos = segments[segmentsCount - 1].position;
            data.rootRot = segments[segmentsCount - 1].rotation;
            string json = JsonUtility.ToJson(data);
            var writer = new StreamWriter(filepath, false);
            writer.WriteLine(json);
            writer.Close();
            Debug.Log("saved: ");
        }

        if (load.IsPressed()) {
            var reader = new StreamReader(filepath);
            string json = reader.ReadLine();
            reader.Close();
            var data = JsonUtility.FromJson<PoseData>(json);
            rots = data.rots;
            segments[segmentsCount - 1].position = data.rootPos;
            segments[segmentsCount - 1].rotation = data.rootRot;
            Debug.Log("loaded" + json);
        }
    }
}
