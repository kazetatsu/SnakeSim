using UnityEngine;

public class Serpent : MonoBehaviour
{
    private MiddleManager manager;
    private int segmentNum;
    private float t = 0f;
    [SerializeField] private float coefT;
    [SerializeField] private float coefI;
    [SerializeField] private float coefSin;

    void Start() {
        manager = GetComponent<MiddleManager>();
        segmentNum = manager.SegmentsCount;
    }


    void Update() {
        t += Time.deltaTime;

        for (int i = 0; i < segmentNum; ++i) {
            float x = coefT * t + coefI * (float)i;
            manager.jointAngles[i] = coefSin * Mathf.Sin(x);
        }
    }
}
