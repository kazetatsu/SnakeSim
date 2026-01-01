// SPDX-FileCopyrightText: 2025 Shinagwa Kazemaru
// SPDX-License-Identifier: BSD-2-Clause license

using UnityEngine;

public class SunRotater : MonoBehaviour
{
    SunMoon sun;

    bool isRotating;
    [SerializeField] float durationBlend;
    float timer;
    float dProgress; // d(progress) / dt
    float targetProgress;


    public void RotateSun(int i) {
        timer = 0f;
        targetProgress = (float)i;
        dProgress = (targetProgress - sun.progress) / durationBlend;
        isRotating = true;
    }


    void Start() {
        sun = GetComponent<SunMoon>();
    }


    void Update() {
        if (!isRotating) return;

        timer += Time.deltaTime;
        sun.progress += dProgress * Time.deltaTime;
        if (timer > durationBlend) {
            sun.progress = targetProgress;
            isRotating = false;
        }
    }
}
