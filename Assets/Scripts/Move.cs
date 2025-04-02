using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    JointsManager _manager;
    InputAction _action;
    float secondJointAng = 0f;
    [SerializeField] float angLimit;
    [SerializeField] float angSpeed;
    [SerializeField] float coefLift;

    List<Queue<float>> jointAngleQs;
    [SerializeField, Range(1,32)] int qCapacity;

    private float currentInput;

    const float maxPropTime = 4f; // propagation
    [SerializeField, Range(0f,4f)] float propTime; 

    float[] angbufTimes;
    float[] angbufValues;
    int angbufNewestIndex = 0;
    const int angbufLength = 256;

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
        // currentInput = _action.ReadValue<Vector2>();

        ++angbufNewestIndex;
        angbufNewestIndex %= angbufLength;

        float dt = Time.deltaTime;

        float currentTime = angbufTimes[angbufNewestIndex] + dt;
        if (currentTime > maxPropTime) currentTime = maxPropTime;
        angbufTimes[angbufNewestIndex] = currentTime;

        // Calcurate target angle of the 2nd joint
        currentInput = _action.ReadValue<float>();
        if (-0.5f < currentInput && currentInput < 0.5f) {
            float sign = Mathf.Sign(secondJointAng);
            secondJointAng -= sign * angSpeed * dt;
            if (sign * secondJointAng < 0f)
                secondJointAng = 0f;
        } else {
            secondJointAng += Mathf.Sign(currentInput) * angSpeed * dt;
            if (secondJointAng < -angLimit)
                secondJointAng = -angLimit;
            else if (secondJointAng > angLimit)
                secondJointAng = angLimit;
        }
        angbufValues[angbufNewestIndex] = secondJointAng;

        _manager.targetRotations[1] = Quaternion.Euler(
            secondJointAng,
            coefLift * Mathf.Abs(secondJointAng),
            0f
        );

        // Calcurate target angle of the 1st joint
        // Look same(near) direction of the main camera
        Vector3 toDirection = _camera.forward;
        toDirection -= Vector3.Dot(_camera.forward, secondSegment.up) * secondSegment.up;
        toDirection = toDirection.normalized;
        if (toDirection.magnitude > Mathf.Epsilon) {
            float radX = Mathf.Asin(Vector3.Dot(toDirection, secondSegment.right));
            _manager.targetRotations[0] = Quaternion.Euler(Mathf.Rad2Deg * radX, 0f, 0f);
        }

        // Calcurate target angle of the 3rd and after joints
        float propSpeed = propTime / (float)_manager.JointsCount;
        float angF = secondJointAng;
        float timeF = currentTime;
        float angB, timeB;
        float timeJoint = currentTime;
        int j = angbufNewestIndex;
        for (int i = 2; i < _manager.JointsCount; ++i) {
            timeJoint -= propSpeed;
            if (timeJoint < 0f) timeJoint += maxPropTime;

            do {
                --j;
                if (j < 0) j = angbufLength - 1;

                timeB = angbufTimes[j];
                angB = angbufValues[j];

                if (
                       (timeB <  timeF && timeB <= timeJoint)
                    || (timeB >= timeF && (timeB <= timeJoint || timeJoint <= timeF))
                ) {


                    timeF = timeB;
                    angF = angB;
                }
            } while (j != angbufNewestIndex);
        }
    }


    void FixedUpdate() {
        // Calcurate target angle of 2nd joint
        if (-0.5f < currentInput && currentInput < 0.5f) {
            float sign = Mathf.Sign(secondJointAng);
            secondJointAng -= sign * angSpeed * Time.fixedDeltaTime;
            if (sign * secondJointAng < 0f)
                secondJointAng = 0f;
        } else {
            secondJointAng += Mathf.Sign(currentInput) * angSpeed * Time.fixedDeltaTime;
            if (secondJointAng < -angLimit)
                secondJointAng = -angLimit;
            else if (secondJointAng > angLimit)
                secondJointAng = angLimit;
        }

        _manager.targetRotations[1] = Quaternion.Euler(
            secondJointAng,
            coefLift * Mathf.Abs(secondJointAng),
            0f
        );

        float futureAng = secondJointAng;
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

        // Calcurate target angle of 1st joint
        // Look same(near) direction of the main camera
        Vector3 toDirection = _camera.forward;
        toDirection -= Vector3.Dot(_camera.forward, secondSegment.up) * secondSegment.up;
        toDirection = toDirection.normalized;
        if (toDirection.magnitude > Mathf.Epsilon) {
            float radX = Mathf.Asin(Vector3.Dot(toDirection, secondSegment.right));
            _manager.targetRotations[0] = Quaternion.Euler(Mathf.Rad2Deg * radX, 0f, 0f);
        }
    }
}
