// SPDX-FileCopyrightText: 2025 Shinagwa Kazemaru
// SPDX-License-Identifier: BSD-2-Clause license

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FirstButtonSelecter : MonoBehaviour
{
    void Start() {
        Button buttonContinue = GameObject.Find("Continue").GetComponent<Button>();

        if (SaveData.HasProgress()) {
            buttonContinue.interactable = true;
            EventSystem.current.SetSelectedGameObject(buttonContinue.gameObject);
        } else {
            buttonContinue.interactable = false;
            EventSystem.current.SetSelectedGameObject(GameObject.Find("NewGame"));
        }
    }
}
