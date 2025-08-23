using UnityEngine;

public class Pose : MonoBehaviour {
    int segmentsCount;
    Transform[] segments;
    [SerializeField] float dist;
    [SerializeField] Quaternion[] rots;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        segmentsCount = transform.childCount;

        segments = new Transform[segmentsCount];
        for (int i = 0; i < segmentsCount; ++i) {
            segments[i] = transform.GetChild(i);
            segments[i].position = ((float)i + 0.5f) * dist * Vector3.back;
        }

        rots = new Quaternion[segmentsCount - 1];
        for (int i = 0; i < segmentsCount - 1; ++i)
            rots[i] = Quaternion.identity;
    }

    // Update is called once per frame
    void Update() {
        for (int i = segmentsCount - 2; i >= 0; --i) {
            segments[i].rotation = rots[i] * segments[i + 1].rotation;
            segments[i].position = 0.5f * dist * (rots[i] * segments[i + 1].forward)
                                 + 0.5f * dist * segments[i + 1].forward
                                 + segments[i + 1].position;
        }
    }
}
