// SPDX-FileCopyrightText: 2025 Shinagwa Kazemaru
// SPDX-License-Identifier: BSD-2-Clause license

using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] float delay;


    void OnTriggerEnter(Collider _) {
        GameObject.Find("SceneLoader")?.GetComponent<SceneLoader>().LoadWithCurtain(delay, "Ending");
        Destroy(this);
    }
}
