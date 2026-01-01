// SPDX-FileCopyrightText: 2025 Shinagwa Kazemaru
// SPDX-License-Identifier: BSD-2-Clause license

#if UNITY_EDITOR
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
            pose.rotations = new Quaternion[n];
            for (int i = 0; i < n; ++i)
                pose.rotations[i] = backs[i].rotation;
            PoseIO.Write(pose, fileName);
        }
    }
}
#endif