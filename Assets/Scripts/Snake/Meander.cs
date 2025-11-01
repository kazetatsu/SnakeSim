using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Meander : MonoBehaviour
{
    JointsRotater joints;
    InputAction action;
    float firstJointAng = 0f;
    [SerializeField] float angLimit;
    [SerializeField] float angSpeed;
    [SerializeField] float coefLift;

    Queue<float>[] jointAngleQs;
    [SerializeField, Range(1,32)] int qCapacity;

    float currentInput;

    HingeJoint[] bellyJoints;

    Transform secondSegment;
    Rigidbody[] backs;


    public void Activate() { enabled = true; }
    public void Deactivate() { enabled = false; }


    void Start() {
        joints = this.GetComponent<JointsRotater>();
        action = InputSystem.actions.FindAction("Meander");

        int count = Snake.JointsCount - 1;
        jointAngleQs = new Queue<float>[count];
        for (int i = 0; i < count; ++i) {
            var q = new Queue<float>(qCapacity);
            for (int j = 0; j < qCapacity; ++j)
                q.Enqueue(0f);
            jointAngleQs[i] = q;
        }

        var _joints = new List<HingeJoint>();
        var _bodys = new List<Rigidbody>();
        for (int i = 0; i < this.transform.childCount; ++i) {
            Transform segment = this.transform.GetChild(i);
            _joints.Add(
                segment
                .Find("Belly")
                .GetComponent<HingeJoint>()
            );

            _bodys.Add(
                segment
                .Find("Back")
                .GetComponent<Rigidbody>()
            );
        }
        bellyJoints = _joints.ToArray();
        backs = _bodys.ToArray();

        secondSegment = this.transform.GetChild(1).Find("Back");

        Deactivate();
    }


    void Update() { currentInput = action.ReadValue<float>(); }


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

        joints.targetRotations[1] = Quaternion.Euler(
            firstJointAng,
            coefLift * Mathf.Abs(firstJointAng),
            0f
        );

        // Set target rotations of 3rd~ joint.
        float futureAng = firstJointAng;
        for (int i = 2; i < Snake.JointsCount; ++i) {
            Queue<float> q = jointAngleQs[i - 1];
            float currentAng = q.Dequeue();
            q.Enqueue(futureAng);

            joints.targetRotations[i] = Quaternion.Euler(
                currentAng,
                coefLift * Mathf.Abs(currentAng),
                0f
            );
            futureAng = currentAng;
        }

        // Calcurate target rotations of 1st joint.
        var toDirection = Vector3.zero;
        for (int i = 1; i <= Snake.JointsCount; ++i)
            toDirection += backs[i].linearVelocity;
        toDirection -= Vector3.Dot(toDirection, secondSegment.up) * secondSegment.up;

        if (toDirection.magnitude > 4f) {
            float radX = Mathf.Asin(Vector3.Dot(toDirection.normalized, secondSegment.right));
            if (float.IsNormal(radX) || radX == 0f)
                joints.targetRotations[0] = Quaternion.Euler(Mathf.Rad2Deg * radX, 0f, 0f);
        }
    }
}
