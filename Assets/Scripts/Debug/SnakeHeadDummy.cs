#if UNITY_EDITOR

using UnityEngine;
using UnityEngine.InputSystem;

public class SnakeHeadDummy : MonoBehaviour
{
    InputAction action;

    [SerializeField] float speed;


    void Start() {
        action = InputSystem.actions.FindActionMap("Debug").FindAction("Move");
    }


    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, 10f);
    }


    void Update() {
        var v = speed * action.ReadValue<Vector3>();
        transform.position += v * Time.deltaTime;
        Snake.headPos = transform.position;
    }
}

#endif