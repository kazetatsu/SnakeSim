using System.Collections.Generic;
using UnityEngine;

public class SegmentsManager : MonoBehaviour
{
    [SerializeField, Range(0, 32)] private int segmentNum;
    public int SegmentNum {get=>segmentNum;}
    [SerializeField, Range(0f, 10f)] private float segmentDist;

    [SerializeField] private GameObject head;
    [SerializeField] private GameObject segmentPrefab;
    private List<ConfigurableJoint> joints;

    public List<float> jointAngles;

    void Start() {
        joints = new List<ConfigurableJoint>();
        jointAngles = new List<float>();

        Transform frontBack = head.transform.Find("Back");

        for (int i = 0; i < segmentNum; ++i) {
            GameObject segment = Instantiate(segmentPrefab, this.transform);
            segment.name = "Segment" + (i+1).ToString("d2");

            segment.transform.localPosition = (float)i * segmentDist * Vector3.back;

            Transform back = segment.transform.Find("Back");
            var joint = back.GetComponent<ConfigurableJoint>();

            Physics.IgnoreCollision(frontBack.GetComponentInChildren<Collider>(), back.GetComponentInChildren<Collider>());
            joint.connectedBody = frontBack.GetComponent<Rigidbody>();

            joints.Add(joint);
            jointAngles.Add(0f);
            frontBack = back;
        }
    }

    void Update() {
        for (int i = 0; i < segmentNum; ++i)
            joints[i].targetRotation = Quaternion.Euler(jointAngles[i], 0f, 0f);
    }
}
