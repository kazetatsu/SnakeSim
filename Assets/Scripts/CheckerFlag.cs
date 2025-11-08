using System.Collections;
using UnityEngine;

public class CheckerFlag : MonoBehaviour
{
    const float RaiseTime = 2f;
    Vector3 initialPos;

    [SerializeField] float angMax;
    [SerializeField] float freq;
    float theta, dthetadt;
    Transform flag;


    IEnumerator RaiseFlag() {
        float t = RaiseTime;
        while (t > 0f) {
            flag.localPosition = (t / RaiseTime) * initialPos;
            yield return null;
            t -= Time.deltaTime;
        }
        flag.localPosition = Vector3.zero;
    }


    public void RaiseFlagGradually() {
        StartCoroutine(RaiseFlag());
    }


    public void RaiseFlagImmediately() {
        flag.localPosition = Vector3.zero;
    }


    void Start() {
        flag = transform.GetChild(0);
        initialPos = flag.localPosition;
        theta = 0f;
        dthetadt = 2f * Mathf.PI * freq;
    }


    void Update() {
        theta += dthetadt * Time.deltaTime;
        if (theta > 2f * Mathf.PI)
            theta -= 2f * Mathf.PI;
        flag.localRotation = Quaternion.AngleAxis(angMax * Mathf.Sin(theta), Vector3.up);
    }
}
