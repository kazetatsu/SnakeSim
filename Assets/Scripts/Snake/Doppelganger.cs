using UnityEngine;

public class Doppelganger : MonoBehaviour
{
    void Update() {
        transform.position = Snake.headPos;
    }
}
