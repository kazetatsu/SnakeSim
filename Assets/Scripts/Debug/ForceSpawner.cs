// SPDX-FileCopyrightText: 2025 Shinagwa Kazemaru
// SPDX-License-Identifier: BSD-2-Clause license

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ForceSpawner : MonoBehaviour
{
    int n;
    List<InputAction> actionsSpawn;
    InputAction actionDebug;

    Spawner spawner;


    void Start() {
        actionsSpawn = new List<InputAction>();
        InputActionMap map = InputSystem.actions.FindActionMap("Debug");
        int i = 0;
        while (true) {
            var action = map.FindAction("Spawn" + i.ToString());
            if (action is null) break;
            actionsSpawn.Add(action);
            ++i;
        }
        n = actionsSpawn.Count;
        actionDebug = map.FindAction("Debug");

        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
    }


    void Update() {
        if (!actionDebug.IsPressed()) return;

        for (int i = 0; i < n; ++i) {
            if (actionsSpawn[i].IsPressed()) {
                spawner.ForceSpawn(i);
                break;
            }
        }
    }
}
