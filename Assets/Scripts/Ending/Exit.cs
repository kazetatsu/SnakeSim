// SPDX-FileCopyrightText: 2025 Shinagwa Kazemaru
// SPDX-License-Identifier: BSD-2-Clause license

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Exit : MonoBehaviour
{
    [SerializeField] float delay;
    float timer = 0f;

    InputAction action;


    void Start() {
        action = InputSystem.actions.FindAction("Spawn");
    }


    void Update() {
        if (EventSystem.current is null) return;

        if (timer < delay) {
            timer += Time.unscaledDeltaTime;
        } else if (action.IsPressed()) {
            GameObject.Find("SceneLoader")?.GetComponent<SceneLoader>().ToTitle();
            Destroy(this);
        }
    }
}
