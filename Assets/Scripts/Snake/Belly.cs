// SPDX-FileCopyrightText: 2025 Shinagwa Kazemaru
// SPDX-License-Identifier: BSD-2-Clause license

using UnityEngine;
using UnityEngine.InputSystem;

public class Belly : MonoBehaviour
{
    HingeJoint joint;
    InputAction action;
    bool shouldStop;

    void Start() {
        joint = GetComponent<HingeJoint>();
        action = InputSystem.actions.FindAction("Move");
    }


    void Update() {
        shouldStop = action is not null && !action.IsPressed();
    }


    void FixedUpdate() {
        joint.useMotor = shouldStop || (joint.velocity <= -0.01f);
    }
}
