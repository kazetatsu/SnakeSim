using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Highlighter : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] string poseFileName;
    Pose pose;
    PoseBone bone;

    bool selected = false;
    EventSystem eventSystem;

    [SerializeField] Color disabledColor;
    [SerializeField] Color normalColor;
    [SerializeField] Color pressedColor;
    Image image;


    public void OnPointerEnter(PointerEventData _) {
        eventSystem.SetSelectedGameObject(gameObject);
    }


    void OnPressed() {
        image.color = pressedColor;
        bone.enabled = false;
        enabled = false;
    }


    void OnEnabled() {
        image.color = normalColor;
    }


    void Start() {
        pose = PoseIO.Read(poseFileName);
        bone = GameObject.Find("SnakePoseBone").GetComponent<PoseBone>();
        eventSystem = EventSystem.current;
        image = transform.Find("Image").GetComponent<Image>();

        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnPressed);
        if (!GetComponent<Button>().interactable)
            image.color = disabledColor;
    }


    void Update() {
        GameObject currentSelectedObj = eventSystem.currentSelectedGameObject;

        // Highlight
        if (!selected && currentSelectedObj == gameObject) {
            bone.SetTargetPose(pose);
            selected = true;
        }

        // Unhighlight
        if (selected && currentSelectedObj != gameObject) {
            selected = false;
        }
    }
}
