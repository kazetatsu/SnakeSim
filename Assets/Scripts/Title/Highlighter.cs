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
    [SerializeField] Color pressedColor;
    TextMeshProUGUI text;


    public void OnPointerEnter(PointerEventData _) {
        eventSystem.SetSelectedGameObject(gameObject);
    }


    void OnPressed() {
        text.color = pressedColor;
        bone.enabled = false;
    }


    void Start() {
        pose = PoseIO.Read(poseFileName);
        bone = GameObject.Find("SnakePoseBone").GetComponent<PoseBone>();
        eventSystem = EventSystem.current;
        text = transform.Find("Text").GetComponent<TextMeshProUGUI>();

        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnPressed);
        if (!GetComponent<Button>().interactable)
            text.color = disabledColor;
    }


    void Update() {
        GameObject currentSelectedObj = eventSystem.currentSelectedGameObject;

        // Highlight
        if (!selected && currentSelectedObj == gameObject) {
            bone.SetTargetPose(pose);
            text.fontStyle = FontStyles.Underline;
            selected = true;
        }

        // Unhighlight
        if (selected && currentSelectedObj != gameObject) {
            text.fontStyle = FontStyles.Normal;
            selected = false;
        }
    }
}
