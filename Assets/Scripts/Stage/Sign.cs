using UnityEngine;

public class Sign : MonoBehaviour
{
    [SerializeField] Texture2D tex;


    void Start() {
        transform.Find("Paper")
            .GetComponent<MeshRenderer>()
            .material
            .SetTexture("_BaseMap", tex);
    }
}
