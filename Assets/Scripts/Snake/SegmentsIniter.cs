using Unity.Cinemachine;
using UnityEngine;

public class SegmentsIniter : MonoBehaviour
{
    [SerializeField] GameObject segmentPrefab;
    [SerializeField] float dist;


    void Start() {
        Transform tail = this.transform.GetChild(2);
        int count = Snake.SegmentsCount - 2; // number of middle segments

        for (int i = 2; i <= count; ++i) {
            var segment = Instantiate(segmentPrefab, this.transform).transform;
            segment.SetSiblingIndex(i);
            segment.name = "Segment " + i.ToString("d2");
            segment.localPosition = (float)i * dist * Vector3.back;
        }

        tail.localPosition = (float)(count+1) * dist * Vector3.back;
    }
}
