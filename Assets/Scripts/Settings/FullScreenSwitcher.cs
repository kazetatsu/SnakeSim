using UnityEngine;
using UnityEngine.UI;

public class FullScreenSwitcher : MonoBehaviour
{
    void OnButtonPressed() {
        Screen.fullScreen = !Screen.fullScreen;
    }


    void Start() {
        GetComponent<Button>().onClick.AddListener(OnButtonPressed);
        GetComponent<HandleToggle>().IsHandleOnLeft = !Screen.fullScreen;
    }
}
