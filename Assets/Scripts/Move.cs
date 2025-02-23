using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    MiddleManager _manager;
    InputAction _action;
    int jointsCount;
    float firstJointAng = 0f;
    [SerializeField] float angLimit;
    [SerializeField] float angSpeed;

    List<Queue<float>> jointAngleQs;
    [SerializeField, Range(1,32)] int qCapacity;

    private Vector2 currentInput;

    void Start() {
        _manager = this.GetComponent<MiddleManager>();
        _action = InputSystem.actions.FindAction("Move");
        jointsCount = _manager.SegmentsCount; // SegmentsManager.Start() must called before this line.

        jointAngleQs = new List<Queue<float>>();
        for (int i = 1; i < jointsCount; ++i) {
            var q = new Queue<float>(qCapacity);
            for (int j = 0; j < qCapacity; ++j)
                q.Enqueue(0f);
            jointAngleQs.Add(q);
        }
    }


    void Update() {
        currentInput = _action.ReadValue<Vector2>();
    }

    void FixedUpdate() {
        if (currentInput.y > 0.1f) {
            // Calcurate target angle of 1st middle segmen.
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

            _manager.jointAngles[0] = firstJointAng;

            float futureAng = firstJointAng;
            for (int i = 1; i < jointsCount; ++i) {
                Queue<float> q = jointAngleQs[i - 1];
                float currentAng = q.Dequeue();
                q.Enqueue(futureAng);

                _manager.jointAngles[i] = currentAng;
                futureAng = currentAng;
            }
        }
    }
}
