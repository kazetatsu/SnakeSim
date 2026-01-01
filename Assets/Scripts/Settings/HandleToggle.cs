// SPDX-FileCopyrightText: 2025 Shinagwa Kazemaru
// SPDX-License-Identifier: BSD-2-Clause license

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HandleToggle : MonoBehaviour
{
    [SerializeField] float durationHandleMove;
    [SerializeField] Vector2 leftPosition;
    [SerializeField] Vector2 rightPosition;
    RectTransform handle;
    bool isHandleOnLeft;
    public bool IsHandleOnLeft {
        get => isHandleOnLeft;
        set {
            if (value)
                handle.anchoredPosition = leftPosition;
            else
                handle.anchoredPosition = rightPosition;
            isHandleOnLeft = value;
        }
    }


    IEnumerator MoveHandle(Vector2 toPosition) {
        Vector2 fromPosition = handle.anchoredPosition;
        float ds = -1f / durationHandleMove;
        float s = 1f;

        while(true) {
            yield return null;
            s += ds * Time.unscaledDeltaTime;
            if (s <= 0f) break;

            handle.anchoredPosition = s * fromPosition + (1f - s) * toPosition;
        }

        handle.anchoredPosition = toPosition;
    }


    void OnButtonPressed() {
        // Move handle L->R
        if (isHandleOnLeft) {
            StartCoroutine(MoveHandle(rightPosition));
            isHandleOnLeft = false;
        // Move handle R->L
        } else {
            StartCoroutine(MoveHandle(leftPosition));
            isHandleOnLeft = true;
        }
    }


    void Start() {
        handle = transform.Find("Handle").GetComponent<RectTransform>();
        isHandleOnLeft = true;
        handle.anchoredPosition = leftPosition;
        GetComponent<Button>().onClick.AddListener(OnButtonPressed);
    }
}
