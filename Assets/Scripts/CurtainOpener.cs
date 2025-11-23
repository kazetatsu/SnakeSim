using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Open/Close curtain
public class CurtainOpener : MonoBehaviour
{
    [SerializeField] float delay;
    [SerializeField] float interval;


    bool isOpening = false;
    float dOpen; // d(open) / dt
    float open;

    Curtain curtain;
    [SerializeField] UnityEvent onTimeout;
    EventSystem eventSystem;


    void Open() {
        eventSystem.enabled = false;
        dOpen = 1f / interval;
        open = -dOpen * delay;
        isOpening = true;
    }


    public void Close() {
        eventSystem.enabled = false;
        dOpen = -1f / interval;
        open = 1f;
        isOpening = true;
    }


    void Start() {
        curtain = GetComponent<Curtain>();
        eventSystem = EventSystem.current;
        Open();
    }


    void Update() {
        if (!isOpening) return;

        open += dOpen * Time.deltaTime;
        if (dOpen < 0f && open < 0f) {
            open = 0f;
            onTimeout?.Invoke();
            isOpening = false;
        }
        if (dOpen > 0f && open > 1f) {
            open = 1f;
            eventSystem.enabled = true;
            isOpening = false;
        }
        curtain.open = open;
    }
}
