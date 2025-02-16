using System.Collections.Generic;
using UnityEngine;

public class SegmentsManager : MonoBehaviour
{
    [SerializeField, Range(0, 32)] private int segmentNum;
    public int SegmentNum {get=>segmentNum;}
    [SerializeField, Range(0f, 10f)] private float segmentDist;

    [SerializeField] private GameObject head;
    [SerializeField] private GameObject segmentPrefab;
    private List<GameObject> segments;
    private List<ConfigurableJoint> joints;

    public List<float> jointAngles;

    void Start() {
        segments = new List<GameObject>();
        joints = new List<ConfigurableJoint>();
        jointAngles = new List<float>();

        for (int i = 0; i < segmentNum; ++i) {
            GameObject segment = Instantiate(segmentPrefab, this.transform);
            segment.name = "Segment" + (i+1).ToString("d2");

            segment.transform.localPosition = (float)(i + 1) * segmentDist * Vector3.back;

            var joint = segment.transform.Find("Back").GetComponent<ConfigurableJoint>();
            GameObject frontObj;
            if (i == 0)
                frontObj = head;
            else
                frontObj = segments[i - 1];
            joint.connectedBody = frontObj.transform.Find("Back").GetComponent<Rigidbody>();

            segments.Add(segment);
            joints.Add(joint);
            jointAngles.Add(0f);
        }
    }

    void Update() {
        for (int i = 0; i < segmentNum; ++i)
            joints[i].targetRotation = Quaternion.Euler(jointAngles[i], 0f, 0f);
    }
}
