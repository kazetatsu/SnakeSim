using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    public bool isEnabled;

    JointsManager _manager;
    InputAction _action;
    float firstJointAng = 0f;
    [SerializeField] float angLimit;
    [SerializeField] float angSpeed;
    [SerializeField] float coefLift;

    List<Queue<float>> jointAngleQs;
    [SerializeField, Range(1,32)] int qCapacity;

    private Vector2 currentInput;

    void Start() {
        _manager = this.GetComponent<JointsManager>();
        _action = InputSystem.actions.FindAction("Move");

        jointAngleQs = new List<Queue<float>>();
        for (int i = 1; i < _manager.JointsCount; ++i) {
            var q = new Queue<float>(qCapacity);
            for (int j = 0; j < qCapacity; ++j)
                q.Enqueue(0f);
            jointAngleQs.Add(q);
        }
    }


    void Update() {
        if (isEnabled)
            currentInput = _action.ReadValue<Vector2>();
    }


    void FixedUpdate() {
        if (isEnabled) {
            // Calcurate target angle of 2nd joint
            if (-0.5f < currentInput.x && currentInput.x < 0.5f) {
                float sign = Mathf.Sign(firstJointAng);
                firstJointAng -= sign * angSpeed * Time.fixedDeltaTime;
                if (sign * firstJointAng < 0f)
                    firstJointAng = 0f;
            } else {
                firstJointAng += Mathf.Sign(currentInput.x) * angSpeed * Time.fixedDeltaTime;
                if (firstJointAng < -angLimit)
                    firstJointAng = -angLimit;
                else if (firstJointAng > angLimit)
                    firstJointAng = angLimit;
            }

            _manager.targetRotations[1] = Quaternion.Euler(
                firstJointAng,
                coefLift * Mathf.Abs(firstJointAng),
                0f
            );

            float futureAng = firstJointAng;
            for (int i = 2; i < _manager.JointsCount; ++i) {
                Queue<float> q = jointAngleQs[i - 1];
                float currentAng = q.Dequeue();
                q.Enqueue(futureAng);

                _manager.targetRotations[i] = Quaternion.Euler(
                    currentAng,
                    coefLift * Mathf.Abs(currentAng),
                    0f
                );
                futureAng = currentAng;
            }
        }
    }
}
