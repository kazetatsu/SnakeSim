using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenShot : MonoBehaviour
{
    InputAction action;
    int cnt = 0;
    [SerializeField] int scaler;


    string pngPath(int i) {
        return Path.Combine(Application.dataPath, "Capture", "Movie" + i.ToString() + ".png");
    }

    void Start() {
        action = InputSystem.actions.FindActionMap("Debug").FindAction("Capture");
        Screen.fullScreen = true;
    }


    void Update() {
        if (action.IsPressed()) {
            ScreenCapture.CaptureScreenshot(pngPath(0), scaler);
            ++cnt;
        }
    }
}
