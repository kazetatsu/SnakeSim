using UnityEngine;

[ExecuteAlways]
public class SunMoon : MonoBehaviour
{
    [SerializeField] public float progress;

    [SerializeField] float[] angles;
    [SerializeField] Color[] colors;
    [SerializeField] float[] strengths;

    Light _light;


    void Start() {
        _light = GetComponent<Light>();
    }


    void Update() {
        int n = Mathf.Min(colors.Length, angles.Length, strengths.Length);

        float angle;
        Color color;
        float strength;

        if (progress <= 0f) {
            angle = angles[0];
            color = colors[0];
            strength = strengths[0];
        } else if (progress >= (float)(n-1)) {
            angle = angles[n-1];
            color = colors[n-1];
            strength = strengths[n-1];
        } else {
            float t = progress;
            int i1 = Mathf.FloorToInt(t);
            int i2 = i1 + 1;
            t -= (float)i1;

            angle = (1f-t) * angles[i1] + t * angles[i2];
            color = Color.Lerp(colors[i1], colors[i2], t);
            strength = (1f-t) * strengths[i1] + t * strengths[i2];
        }

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.right);
        if (transform.forward.y > 0f) {
            transform.rotation = Quaternion.AngleAxis(angle + 180f, Vector3.right);
        }
        _light.color = color;
        _light.shadowStrength = Mathf.Abs(strength);
    }
}
