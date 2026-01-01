// SPDX-FileCopyrightText: 2025 Shinagwa Kazemaru
// SPDX-License-Identifier: BSD-2-Clause license

using UnityEngine;

public class Head : MonoBehaviour
{
    void Update() {
        Snake.headPos = transform.position;
    }
}
