using UnityEngine;
using UnityEngine.InputSystem;

public class PoseImporter : MonoBehaviour
{
    InputAction action;
    [SerializeField] string fileName;
    PoseBone bone;


    void Start() {
        action = InputSystem.actions.FindActionMap("Debug").FindAction("Import");
        bone = GetComponent<PoseBone>();
    }


    void Update() {
        if (action.IsPressed()) {
            Pose pose = PoseIO.Read(fileName);
            bone.SetTargetPose(pose);
        }
    }
}
