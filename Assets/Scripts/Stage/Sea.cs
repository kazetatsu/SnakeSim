using UnityEngine;

public class Sea : MonoBehaviour
{
    const float TAU = 2f * Mathf.PI;
    Material _material;
    Light sun;
    [SerializeField] float speed;
    float offset = 0f;


    void Start() {
        _material = transform.GetChild(0).GetComponent<MeshRenderer>().material;
        sun = GameObject.Find("Sun").GetComponent<Light>();
    }


    void Update() {
        offset += speed * Time.deltaTime;
        if (offset > 1f)
            offset -= 1f;;
        _material.SetColor("_SunColor", sun.color);
        _material.SetFloat("_Offset", offset);
    }
}
