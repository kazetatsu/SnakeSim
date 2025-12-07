using System.Collections;
using UnityEngine;

public class MenuCursor : MonoBehaviour
{
    [SerializeField] float durationMove;
    RectTransform rectTransform;


    IEnumerator ChangePositionY(float toY) {
        float fromY = rectTransform.anchoredPosition.y;

        float ds = -1f / durationMove;
        float s = 1f;
        Vector2 temp;

        while (true) {
            yield return null;
            s += ds * Time.deltaTime;
            if (s <= 0f) break;

            temp = rectTransform.anchoredPosition;
            temp.y = s * fromY + (1f - s) * toY;
            rectTransform.anchoredPosition = temp;
        }

        temp = rectTransform.anchoredPosition;
        temp.y = toY;
        rectTransform.anchoredPosition = temp;
    }


    public void Move(float toPositionY) {
        StartCoroutine(ChangePositionY(toPositionY));
    }


    void Start() {
        rectTransform = GetComponent<RectTransform>();
    }
}
