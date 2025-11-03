using UnityEngine;
using UnityEngine.InputSystem;

public class Head : MonoBehaviour
{
    void Update() {
        Snake.headPos = transform.position;
    }
}
