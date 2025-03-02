using UnityEngine;
using UnityEngine.InputSystem;

public class Look : MonoBehaviour
{
    const int LookJointsCount = 3;
    const float InvCount = 1f / (float)LookJointsCount;

    InputAction _actionPad;
    InputAction _actionMouse;
    JointsManager _manager;
    float angLift = 0f;
    [SerializeField] float angLiftLimit; 
    [SerializeField] float angLiftSpeed;
    float angYaw = 0f;
    [SerializeField] float angYawLimit; 
    [SerializeField] float angYawSpeed;

    Vector2 currentInput;
    [SerializeField] float sensitivityPad;
    [SerializeField] float sensitivityMouse;

    public void Activate() {
        enabled = true;
    }

    public void Deactivate() {
        angLift = 0f;
        angYaw = 0f;
        enabled = false;
    }

    void Start() {
        _actionPad   = InputSystem.actions.FindAction("LookPad");
        _actionMouse = InputSystem.actions.FindAction("LookMouse");
        _manager = this.transform.GetComponent<JointsManager>();
        Deactivate();
    }

    void Update() {
        currentInput = sensitivityPad * _actionPad.ReadValue<Vector2>();
        currentInput += sensitivityMouse * _actionMouse.ReadValue<Vector2>() / (float)Screen.width;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate() {
        if (Mathf.Abs(currentInput.y) < 0.1f) {
            if (Mathf.Abs(angLift) < 5f)
                angLift = 0f;
        } else {
            angLift += currentInput.y * angLiftSpeed * Time.fixedDeltaTime;
            if (angLift < -angLiftLimit) angLift = -angLiftLimit;
            else if (angLift > angLiftLimit) angLift = angLiftLimit;
        }

        if (Mathf.Abs(currentInput.x) < 0.1f) {
            if (Mathf.Abs(angYaw) < 5f)
                angYaw = 0f;
        } else {
            angYaw += currentInput.x * angYawSpeed * Time.fixedDeltaTime;
            if (angYaw > angYawLimit) angYaw = angYawLimit;
            else if (angYaw < -angYawLimit) angYaw = -angYawLimit;
        }


        var rot = Quaternion.Euler(angYaw * InvCount, angLift * InvCount, 0f);
        for (int i = 0; i < LookJointsCount; ++i)
            _manager.targetRotations[i] = rot;
    }
}
