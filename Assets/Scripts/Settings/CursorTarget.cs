using UnityEngine;
using UnityEngine.EventSystems;

public class CursorTarget : MonoBehaviour
{
    bool selected = false;
    MenuCursor cursor;
    float positionY;


    void Start() {
        cursor = transform.parent.Find("Cursor")?.GetComponent<MenuCursor>();
        positionY = GetComponent<RectTransform>().anchoredPosition.y;
    }


    void Update() {
        GameObject currentSelectedObj = EventSystem.current?.currentSelectedGameObject;

        // Highlight
        if (!selected && currentSelectedObj == gameObject) {
            cursor?.Move(positionY);
            selected = true;
        }

        // Unhighlight
        if (selected && currentSelectedObj != gameObject) {
            selected = false;
        }
    }
}
