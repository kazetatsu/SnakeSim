// SPDX-FileCopyrightText: 2025 Shinagwa Kazemaru
// SPDX-License-Identifier: BSD-2-Clause license

using UnityEngine;
using UnityEngine.EventSystems;

public class PoseOnSelected : MonoBehaviour
{
    [SerializeField] string poseFileName;
    Pose pose;
    PoseBone bone;

    bool selected = false;
    EventSystem eventSystem;


    void Start() {
        pose = PoseIO.Read(poseFileName);
        bone = GameObject.Find("SnakePoseBone").GetComponent<PoseBone>();
        eventSystem = EventSystem.current;
    }


    void Update() {
        GameObject currentSelectedObj = eventSystem.currentSelectedGameObject;

        // Highlight
        if (!selected && currentSelectedObj == gameObject) {
            bone.SetTargetPose(pose);
            selected = true;
        }

        // Unhighlight
        if (selected && currentSelectedObj != gameObject) {
            selected = false;
        }
    }
}
