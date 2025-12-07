using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HandleToggle : MonoBehaviour
{
    [SerializeField] float durationHandleMove;
    [SerializeField] Vector2 leftPosition;
    [SerializeField] Vector2 rightPosition;
    RectTransform handle;
    public bool isHandleOnLeft;


    IEnumerator MoveHandle(Vector2 toPosition) {
        Vector2 fromPosition = handle.anchoredPosition;
        float ds = -1f / durationHandleMove;
        float s = 1f;

        while(true) {
            yield return null;
            s += ds * Time.deltaTime;
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
