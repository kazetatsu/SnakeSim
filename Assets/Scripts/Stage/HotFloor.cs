using UnityEngine;

public class HotFloor : MonoBehaviour
{
    int index;
    Heat heat;


    void Start() {
        index = transform.GetSiblingIndex();
        heat = transform.parent.GetComponent<Heat>();
    }
    void Update() {}


    void OnCollisionEnter(Collision collision) {
        heat.OnSnakeTouch(index, collision.transform.parent);
    }

    void OnCollisionExit(Collision collision) {
        heat.OnSnakeLeave(index, collision.transform.parent);
    }
}
