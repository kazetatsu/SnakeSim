using UnityEngine;
using UnityEngine.InputSystem;

public class Pusher : MonoBehaviour
{
    Rigidbody _rigidbody;
    InputAction _action;
    Vector2 pushDirection;
    float pushStrength;
    [SerializeField, Range(0f, 10f)] float defaultForce;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
        _action = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update() {
        if (_action == null) return;

        pushDirection = _action.ReadValue<Vector2>();
        pushStrength = pushDirection.magnitude;
        if (pushStrength > 1f) {
            pushDirection /= pushStrength;
        }
    }

    void FixedUpdate() {
        if (pushStrength > 0f) {
            Vector3 force = defaultForce * this.transform.TransformDirection(pushDirection.x, 0f, pushDirection.y);
            _rigidbody.AddForce(force);
        }
    }
}
