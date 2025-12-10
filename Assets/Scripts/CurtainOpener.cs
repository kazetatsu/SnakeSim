using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Open/Close curtain
public class CurtainOpener : MonoBehaviour {
    [SerializeField] float delay;
    [SerializeField] float duration;

    float dOpen; // d(open) / dt
    float open;

    Curtain curtain;
    Action onClosed;
    EventSystem eventSystem;


    void Open() {
        eventSystem.enabled = false;
        dOpen = 1f / duration;
        open = -dOpen * delay;
        StartCoroutine(OpenClose());
    }


    public void Close(float delay, Action onClosed) {
        eventSystem.enabled = false;
        dOpen = -1f / duration;
        open = 1f - dOpen * delay;
        this.onClosed = onClosed;
        StartCoroutine(OpenClose());
    }


    IEnumerator OpenClose() {
        while (true) {
            yield return null;
            open += dOpen * Time.unscaledDeltaTime;
            curtain.open = open;
            if (dOpen < 0f && open < 0f) {
                yield return null;
                onClosed?.Invoke();
                break;
            }
            if (dOpen > 0f && open > 1f) {
                eventSystem.enabled = true;
                break;
            }
        }
    }


    void Start() {
        curtain = GetComponent<Curtain>();
        eventSystem = EventSystem.current;
        Open();
    }
    void Update() {}
}
