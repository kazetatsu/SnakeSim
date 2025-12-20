using UnityEngine;

[ExecuteAlways]
public class AspectKeeper : MonoBehaviour
{
    Camera _camera;


    void Start() {
        Application.targetFrameRate = 60;
        _camera = GetComponent<Camera>();
    }


    void Update() {
        float w, h;
        if (Screen.width > Screen.height) {
            w = (float)Screen.height / (float)Screen.width;
            h = 1f;
        } else {
            w = 1f;
            h = (float)Screen.width / (float)Screen.height;
        }

        _camera.rect = new Rect(0.5f - w*0.5f, 0.5f - h*0.5f, w, h);
    }
}
