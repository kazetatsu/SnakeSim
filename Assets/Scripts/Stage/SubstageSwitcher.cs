// SPDX-FileCopyrightText: 2025 Shinagwa Kazemaru
// SPDX-License-Identifier: BSD-2-Clause license

using UnityEngine;

public class SubstageSwitcher : MonoBehaviour
{
    Substage[] substages;
    int nearestID = 0;
    int priorID = 0;

    const float SwitchDelay = .1f;
    float timeTillSwitch;


    void Start() {
        var substageObjs = GameObject.FindGameObjectsWithTag("Substage");
        int n = substageObjs.Length;
        substages = new Substage[n];
        for (int i = 0; i < n; ++i) {
            substages[i] = substageObjs[i].GetComponent<Substage>();
            if (i == priorID)
                substages[i].Prioritize();
            else
                substages[i].Deprioritize();
        }
    }

    void Update() {
        int nearestNew = 0;
        float dMin = float.PositiveInfinity;
        for (int i = 0; i < substages.Length; ++i) {
            float d = substages[i].GetDistanceFromSnake();
            if (d < dMin) {
                nearestNew = i;
                dMin = d;
            }
        }

        if (priorID != nearestNew) {
            //    nearest camera of former frame
            // is nearest camera of current frame ?
            if (nearestID == nearestNew)
                timeTillSwitch -= Time.deltaTime;
            else
                timeTillSwitch = SwitchDelay;
            nearestID = nearestNew;
            if (timeTillSwitch <= 0f) {
                substages[priorID].Deprioritize();
                priorID = nearestID;
                substages[priorID].Prioritize();
            }
        }
    }
}
