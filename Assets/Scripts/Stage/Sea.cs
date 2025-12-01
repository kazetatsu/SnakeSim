using UnityEngine;

public class Sea : MonoBehaviour
{
    Material _material;
    [SerializeField] float speed;
    float offset = 0f;


    void Start() {
        _material = transform.GetChild(0).GetComponent<MeshRenderer>().material;
    }


    void Update() {
        offset += speed * Time.deltaTime;
        if (offset > 1f)
            offset -= 1f;;
        _material.SetTextureOffset("_BaseMap", new Vector2(offset, 0f));
    }
}
