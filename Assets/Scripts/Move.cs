using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    JointsManager _manager;
    InputAction _action;
    float firstJointAng = 0f;
    [SerializeField] float angLimit;
    [SerializeField] float angSpeed;
    [SerializeField] float coefLift;

    List<Queue<float>> jointAngleQs;
    [SerializeField, Range(1,32)] int qCapacity;

    float currentInput;

    HingeJoint[] bellyJoints;

    Transform secondSegment;
    [SerializeField] Transform _camera;

    public void Activate() {
        foreach (HingeJoint belly in bellyJoints)
            belly.useMotor = false;
        enabled = true;
    }

    public void Deactivate() {
        foreach (HingeJoint belly in bellyJoints)
            belly.useMotor = true;
        enabled = false;
    }

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

        var _joints = new List<HingeJoint>();
        for (int i = 0; i < this.transform.childCount; ++i) {
            _joints.Add(
                this.transform
                .GetChild(i)
                .Find("Belly")
                .GetComponent<HingeJoint>()
            );
        }
        bellyJoints = _joints.ToArray();

        secondSegment = this.transform.GetChild(1).Find("Back");

        Deactivate();
    }


    void Update() {
        currentInput = _action.ReadValue<float>();
    }


    void FixedUpdate() {
        // Calcurate target angle of 2nd joint
        if (-0.5f < currentInput && currentInput < 0.5f) {
            float sign = Mathf.Sign(firstJointAng);
            firstJointAng -= sign * angSpeed * Time.fixedDeltaTime;
            if (sign * firstJointAng < 0f)
                firstJointAng = 0f;
        } else {
            firstJointAng += Mathf.Sign(currentInput) * angSpeed * Time.fixedDeltaTime;
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

        Vector3 toDirection = _camera.forward;
        toDirection -= Vector3.Dot(_camera.forward, secondSegment.up) * secondSegment.up;
        toDirection = toDirection.normalized;
        if (toDirection.magnitude > Mathf.Epsilon) {
            float radX = Mathf.Asin(Vector3.Dot(toDirection, secondSegment.right));
            _manager.targetRotations[0] = Quaternion.Euler(Mathf.Rad2Deg * radX, 0f, 0f);
        }
    }
}
