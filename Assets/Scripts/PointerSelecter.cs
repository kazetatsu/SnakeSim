using UnityEngine;
using UnityEngine.EventSystems;

public class PointerSelecter : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData _) {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
