// SPDX-FileCopyrightText: 2025 Shinagwa Kazemaru
// SPDX-License-Identifier: BSD-2-Clause license

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Escape : MonoBehaviour
{
    InputAction action;
    SettingsMenu menu;

    void Start() {
        Time.timeScale = 1f;
        action = InputSystem.actions.FindAction("Escape");
        menu = FindFirstObjectByType<SettingsMenu>(FindObjectsInactive.Include);
    }


    void Update() {
        if (EventSystem.current is null || menu.gameObject.activeSelf) return;

        if (action.IsPressed()) {
            menu.Open();
            Time.timeScale = 0f;
        } else if (Time.timeScale < 1f)
            Time.timeScale = 1f;
    }
}
