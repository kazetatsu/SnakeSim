using UnityEngine;

public class Wood : MonoBehaviour
{
    [SerializeField] Vector2 scaler;
    [SerializeField] Vector2 offset;

    void Start() {
        Material material = GetComponent<MeshRenderer>().material;
        material.SetTextureScale("_BaseMap", scaler);
        material.SetTextureOffset("_BaseMap", offset);
    }
    void Update() {}
}
