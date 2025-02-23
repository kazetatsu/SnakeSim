using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MiddleManager : MonoBehaviour
{
    [SerializeField] private GameObject conectedHead;
    [SerializeField] private GameObject segmentPrefab;
    [SerializeField, Range(1, 32)] private int segmentsCount;
    public int SegmentsCount { get => segmentsCount; }
    [SerializeField, Range(0f, 10f)] private float segmentsDist;
    private List<ConfigurableJoint> joints;
    public List<float> jointAngles;
    [SerializeField] float angLimit;
    [SerializeField] float coefLift;

    void Start() {
        joints = new List<ConfigurableJoint>();
        jointAngles = new List<float>();

        Transform frontBack = conectedHead.transform.Find("Back");

        for (int i = 0; i < segmentsCount; ++i) {
            GameObject segment = Instantiate(segmentPrefab, this.transform);
            segment.name = "Middle" + (i+1).ToString("d2");

            segment.transform.localPosition = (float)i * segmentsDist * Vector3.back;

            Transform back = segment.transform.Find("Back");
            var joint = back.GetComponent<ConfigurableJoint>();

            Physics.IgnoreCollision(frontBack.GetComponentInChildren<Collider>(), back.GetComponentInChildren<Collider>());

            joint.connectedBody = frontBack.GetComponent<Rigidbody>();

            float coefSpring = 1f - (float)i / (float)(segmentsCount - 1);
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

            joints.Add(joint);
            jointAngles.Add(0f);
            frontBack = back;
        }
    }

    void FixedUpdate() {
        for (int i = 0; i < segmentsCount; ++i) {
            float ang = jointAngles[i];
            if (ang < -angLimit)
                ang = -angLimit;
            else if (ang > angLimit)
                ang = angLimit;

            var rot = Quaternion.Euler(ang, coefLift * (angLimit - Mathf.Abs(ang)), 0f);
            joints[i].targetRotation = rot;
        }
    }
}
