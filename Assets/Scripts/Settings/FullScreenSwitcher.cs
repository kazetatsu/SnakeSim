// SPDX-FileCopyrightText: 2025 Shinagwa Kazemaru
// SPDX-License-Identifier: BSD-2-Clause license

using UnityEngine;
using UnityEngine.UI;

public class FullScreenSwitcher : MonoBehaviour
{
    void OnButtonPressed() {
        Screen.fullScreen = !Screen.fullScreen;
    }


    void Start() {
        GetComponent<Button>().onClick.AddListener(OnButtonPressed);
        GetComponent<HandleToggle>().IsHandleOnLeft = !Screen.fullScreen;
    }
}
