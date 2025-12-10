using UnityEngine;

public class Wood : MonoBehaviour
{
    [SerializeField] Vector2 scaler;
    [SerializeField] Vector2 offset;
    #if UNITY_EDITOR
    Material _material;
    #endif

    void Start() {
        #if UNITY_EDITOR
        _material = GetComponent<MeshRenderer>().material;
        #else
        Material material = GetComponent<MeshRenderer>().material;
        material.SetTextureScale("_BaseMap", scaler);
        material.SetTextureOffset("_BaseMap", offset);
        #endif
    }


    void Update() {
        #if UNITY_EDITOR
        _material.SetTextureScale("_BaseMap", scaler);
        _material.SetTextureOffset("_BaseMap", offset);
        #endif
    }
}
