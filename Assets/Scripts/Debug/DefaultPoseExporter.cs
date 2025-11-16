using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DefaultPoseExporter : MonoBehaviour
{
    int aNum, segNum;
    float aSup, da, ainf;
    [SerializeField] float a;
    Vector3[,] jointParams;

    LineRenderer debugLine;
    InputAction action;

    void Start() {
        action = InputSystem.actions.FindActionMap("Debug").FindAction("Export");
        List<float[]> data = CSVIO.Read("ManualJointParams.csv");

        aNum = data.Count;
        segNum = data[0].Length/2;
        aSup = 0.5f * Snake.SegmentsDist * (float)(2 * segNum + 1);
        ainf = aSup * 0.5f;
        da = (aSup - ainf) / (float)(aNum-1);

        jointParams = new Vector3[aNum, segNum];
        for (int i = 0; i < aNum; ++i)
            for (int j = 0; j < segNum; ++j)
                jointParams[i,j] = new Vector3(data[i][2*j+1], 0f, data[i][2*j]);

        debugLine = GetComponent<LineRenderer>();
    }


    void Update() {
        debugLine.positionCount = 2 * (segNum + 1);
        var ps = new Vector3[2 * (segNum + 1)];
        float i = (a - ainf) / da;
        int id, iu;
        float t, ze;
        if (a <= ainf) {
            id = 0;
            iu = 0;
            t = 0f;
            ze = ainf;
        } else if (a >= aSup) {
            id = aNum-1;
            iu = aNum-1;
            t = 0f;
            ze = aSup;
        } else {
            id = Mathf.FloorToInt(i);
            iu = Mathf.CeilToInt(i);
            t = i - (float)id;
            ze = a;
        }
        ps[2*segNum+1] = new Vector3(0f, 0f, ze);
        for (int j = 0; j < segNum; ++j)
            ps[segNum + 1 + j] = (1f-t) * jointParams[id,j] + t * jointParams[iu,j];
        for (int j = 0; j <= segNum; ++j)
            ps[segNum - j] = - ps[segNum + 1 + j];
        debugLine.SetPositions(ps);

        if (action.IsPressed()) {
            var pose = new Pose();
            pose.poss = new Vector3[Snake.SegmentsCount];
            pose.rots = new Quaternion[Snake.SegmentsCount];
            pose.poss[0] = ps[ps.Length - 1] + 0.5f * Snake.SegmentsDist * Vector3.forward;
            for (int j = ps.Length - 2; j >= 0; --j) {
                pose.poss[ps.Length - 1 - j] = (ps[j+1] + ps[j]) * 0.5f;
                pose.rots[ps.Length - 1 - j] = Quaternion.FromToRotation(Vector3.forward, ps[j+1] - ps[j]);
            }
            Vector3 u = ps[0];
            for (int j = ps.Length; j < Snake.SegmentsCount; ++j) {
                pose.poss[j] = ps[0] - ((float)(j - ps.Length) + 0.5f) * Snake.SegmentsDist * Vector3.forward;
                pose.rots[j] = Quaternion.identity;
            }
            PoseIO.Write(pose, "DefaultPose.json");
        }
    }
}
