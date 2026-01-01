// SPDX-FileCopyrightText: 2025 Shinagwa Kazemaru
// SPDX-License-Identifier: BSD-2-Clause license

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] float durationOpen;
    GameObject selectedBefore; // Selected gameobject before open settings menu
    [SerializeField] GameObject firstSelected; // selected gameobject when open settings menu

    RectTransform rectTransform;


    IEnumerator ChangeScale(bool open) {
        // Turn off event system while menu is appearing/hiding
        var eventSystem = EventSystem.current;
        eventSystem.enabled = false;

        float fromScale = rectTransform.localScale.x;
        float toScale = open ? 1f : 0f;
        Vector3 temp;
        float ds = -1f / durationOpen;
        float s = 1f;

        while (true) {
            yield return null;
            s += ds * Time.unscaledDeltaTime;
            if (s <= 0f) break;

            temp = rectTransform.localScale;
            temp.x = s * fromScale + (1f - s) * toScale;
            rectTransform.localScale = temp;
        }

        temp = rectTransform.localScale;
        temp.x = toScale;
        rectTransform.localScale = temp;

        eventSystem.enabled = true;
        if (open) {
            eventSystem.SetSelectedGameObject(firstSelected);
        } else {
            eventSystem.SetSelectedGameObject(selectedBefore);
            gameObject.SetActive(false);
        }
    }


    public void Open() {
        gameObject.SetActive(true);
        selectedBefore = EventSystem.current?.currentSelectedGameObject;
        StartCoroutine(ChangeScale(true));
    }


    void Close() {
        StartCoroutine(ChangeScale(false));
    }


    void Start() {
        rectTransform = GetComponent<RectTransform>();
        Vector2 temp = rectTransform.localScale;
        temp.x = 0f;
        rectTransform.localScale = temp;
        transform.Find("Close").GetComponent<Button>().onClick.AddListener(Close);
        gameObject.SetActive(false);
    }
}
