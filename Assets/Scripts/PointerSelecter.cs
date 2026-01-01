// SPDX-FileCopyrightText: 2025 Shinagwa Kazemaru
// SPDX-License-Identifier: BSD-2-Clause license

using UnityEngine;
using UnityEngine.EventSystems;

public class PointerSelecter : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData _) {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
