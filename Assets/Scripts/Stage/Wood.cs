using UnityEngine;

public class Wood : MonoBehaviour
{
    [SerializeField] Vector2 scaler;

    void Start() {
        GetComponent<MeshRenderer>().material.SetTextureScale("_BaseMap", scaler);
    }
    void Update() {}
}
