using UnityEngine;
using UnityEngine.EventSystems;

public class CursorTarget : MonoBehaviour
{
    bool selected = false;
    EventSystem eventSystem;
    MenuCursor cursor;
    float positionY;


    void Start() {
        eventSystem = EventSystem.current;
        cursor = transform.parent.Find("Cursor")?.GetComponent<MenuCursor>();
        positionY = GetComponent<RectTransform>().anchoredPosition.y;
    }


    void Update() {
        GameObject currentSelectedObj = eventSystem.currentSelectedGameObject;

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
