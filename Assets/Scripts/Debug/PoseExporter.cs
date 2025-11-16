using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class PoseExporter : MonoBehaviour
{
    InputAction action;
    [SerializeField] string fileName;
    Transform[] backs;


    void Start() {
        action = InputSystem.actions.FindActionMap("Debug").FindAction("Export");
        int n = transform.childCount;
        backs = new Transform[n];
        for (int i = 0; i < n; ++i)
            backs[i] = transform.GetChild(i).Find("Back");
    }


    void Update() {
        if (action.IsPressed()) {
            int n = backs.Length;
            var pose = new Pose();
            pose.poss = new Vector3[n];
            pose.rots = new Quaternion[n];
            for (int i = 0; i < n; ++i) {
                pose.poss[i] = backs[i].position;
                pose.rots[i] = backs[i].rotation;
            }
            PoseIO.Write(pose, fileName);
        }
    }
}
