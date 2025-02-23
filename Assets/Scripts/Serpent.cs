using UnityEngine;

public class Serpent : MonoBehaviour
{
    private SegmentsManager manager;
    private int segmentNum;
    private float t = 0f;
    [SerializeField] private float coefT;
    [SerializeField] private float coefI;
    [SerializeField] private float coefSin;

    void Start() {
        manager = GetComponent<SegmentsManager>();
        segmentNum = manager.MiddlesCount;
    }


    void Update() {
        t += Time.deltaTime;

        for (int i = 0; i < segmentNum; ++i) {
            float x = coefT * t + coefI * (float)i;
            manager.middleJointAngles[i] = coefSin * Mathf.Sin(x);
        }
    }
}
