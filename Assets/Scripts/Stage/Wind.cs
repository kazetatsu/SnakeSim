using Unity.VisualScripting;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] Vector3 pushForce;

    Rigidbody[] segmentBodys;
    bool[] isSegmentInside;


    void Start() {
        isSegmentInside = new bool[Snake.SegmentsCount];
        segmentBodys = new Rigidbody[Snake.SegmentsCount];

        for (int i = 0; i < Snake.SegmentsCount; ++i)
            isSegmentInside[i] = false;
    }


    void Update() {}


    void OnTriggerEnter(Collider other) {
        Transform segment = other.transform.parent.parent;
        int i = segment.GetSiblingIndex(); // index of hit segment
        if (segmentBodys[i] is null || segmentBodys[i].IsDestroyed())
            segmentBodys[i] = segment.GetChild(0).GetComponent<Rigidbody>();
        isSegmentInside[i] = true;
    }


    void OnTriggerExit(Collider other) {
        int i = other.transform.parent.parent.GetSiblingIndex(); // index of hit segment
        isSegmentInside[i] = false;
    }


    void FixedUpdate() {
        for (int i = 0; i < Snake.SegmentsCount; ++i) {
            if (segmentBodys[i] is null || segmentBodys[i].IsDestroyed())
                isSegmentInside[i] = false;

            if (!isSegmentInside[i]) continue;

            segmentBodys[i].AddForce(pushForce, ForceMode.Force);
        }
    }
}
