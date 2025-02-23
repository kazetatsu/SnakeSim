using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SegmentsManager : MonoBehaviour
{
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject middlePrefab;
    [SerializeField, Range(1, 32)] private int middlesCount;
    public int MiddlesCount { get => middlesCount; }
    [SerializeField, Range(0f, 10f)] private float middleDist;
    private List<ConfigurableJoint> middleJoints;
    public List<float> middleJointAngles;
    [SerializeField] float angLimit;
    [SerializeField] float coefLift;

    void Start() {
        middleJoints = new List<ConfigurableJoint>();
        middleJointAngles = new List<float>();

        Transform frontBack = head.transform.Find("Back");

        for (int i = 0; i < middlesCount; ++i) {
            GameObject segment = Instantiate(middlePrefab, this.transform);
            segment.name = "Middle" + (i+1).ToString("d2");

            segment.transform.localPosition = (float)i * middleDist * Vector3.back;

            Transform back = segment.transform.Find("Back");
            var joint = back.GetComponent<ConfigurableJoint>();

            Physics.IgnoreCollision(frontBack.GetComponentInChildren<Collider>(), back.GetComponentInChildren<Collider>());

            joint.connectedBody = frontBack.GetComponent<Rigidbody>();

            float coefSpring = 1f - (float)i / (float)(middlesCount - 1);
            coefSpring *= 0.9f;
            coefSpring += 0.1f;
            var _drive = new JointDrive();
            _drive.positionSpring = 100f * coefSpring;
            _drive.maximumForce = 1000f;
            joint.angularXDrive = _drive;
            _drive = new JointDrive();
            _drive.positionSpring = 30f * coefSpring;
            _drive.maximumForce = 300f;
            joint.angularYZDrive = _drive;

            middleJoints.Add(joint);
            middleJointAngles.Add(0f);
            frontBack = back;
        }
    }

    void FixedUpdate() {
        for (int i = 0; i < middlesCount; ++i) {
            float ang = middleJointAngles[i];
            if (ang < -angLimit)
                ang = -angLimit;
            else if (ang > angLimit)
                ang = angLimit;

            var rot = Quaternion.Euler(ang, coefLift * (angLimit - Mathf.Abs(ang)), 0f);
            middleJoints[i].targetRotation = rot;
        }
    }
}
