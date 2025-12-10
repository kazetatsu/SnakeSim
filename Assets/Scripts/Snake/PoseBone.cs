using UnityEngine;

public class PoseBone : MonoBehaviour
{
    Transform[] backs;

    [SerializeField] float timeMove;
    bool isMoving = false;
    float timer;
    Pose targetPose;
    Quaternion[] fromRotations;


    public void SetTargetPose(Pose pose) {
        for (int i = 0; i < Snake.SegmentsCount; ++i)
            fromRotations[i] = backs[i].localRotation;

        targetPose = pose;
        isMoving = true;
        timer = 0f;
    }


    void Start() {
        backs = new Transform[Snake.SegmentsCount];
        for (int i = 0; i < Snake.SegmentsCount; ++i) {
            backs[i] = transform.GetChild(i).Find("Back");
            backs[i].localPosition = ((float)(Snake.SegmentsCount-i) - 0.5f) * Snake.SegmentsDist * Vector3.forward;
        }

        fromRotations = new Quaternion[Snake.SegmentsCount];
    }


    void Update() {
        if (!isMoving) return;

        timer += Time.unscaledDeltaTime;

        if (timer >= timeMove) {
            for (int i = 0; i < Snake.SegmentsCount; ++i)
                backs[i].localRotation = targetPose.rotations[i];
            isMoving = false;
        } else {
            float t = timer / timeMove;
            for (int i = 0; i < Snake.SegmentsCount; ++i) {
                backs[i].localRotation = Quaternion.Lerp(
                    fromRotations[i],
                    targetPose.rotations[i],
                    t
                );
            }
        }

        var p = Vector3.zero;
        for (int i = Snake.SegmentsCount-1; i >= 0; --i) {
            Vector3 f = backs[i].localRotation * Vector3.forward;
            backs[i].localPosition = p + 0.5f * Snake.SegmentsDist * f;
            p += Snake.SegmentsDist * f;
        }
    }
}
