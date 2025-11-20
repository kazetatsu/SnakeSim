using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManualBone : MonoBehaviour
{
    const int SegmentSep = 12;

    InputAction action;

    Transform[] backs;

    float[] jointAngs;
    [SerializeField, Range(10f, 360f)] float xAngStraight;
    [SerializeField] float yAng;
    [SerializeField] float xAng;
    [SerializeField] float yAngMax;
    [SerializeField] float yAngMin;
    [SerializeField] float xAngMax;
    [SerializeField] float xAngMin;

    [SerializeField] float yAngSpeed;
    [SerializeField] float xAngSpeed;


    void Start() {
        action = InputSystem.actions.FindAction("Dance");

        int n = transform.childCount;
        backs = new Transform[n];
        for (int i = 0; i < n; ++i)
            backs[i] = transform.GetChild(i).Find("Back");

        // Get default pose, joints' angles
        Pose defaultPose = PoseIO.Read("DefaultPose.json");
        jointAngs = new float[SegmentSep];
        float ap, an; // prev,next
        ap = 0f;
        for (int i = 0; i < SegmentSep; ++i) {
            defaultPose.rotations[SegmentSep-i-1].ToAngleAxis(out float ang, out Vector3 ax);
            an = -ang * ax.y;
            jointAngs[i] = an - ap;
            ap = an;
        }

        //Reshape back part of the snake
        for (int i = Snake.SegmentsCount-1; i >= SegmentSep; --i)
            backs[i].localPosition = (float)(Snake.SegmentsCount-1 - i) * Snake.SegmentsDist * Vector3.forward;
    }


    void Update() {
        var v = action.ReadValue<Vector2>();
        yAng -= v.x * yAngSpeed * Time.deltaTime;
        xAng -= v.y * xAngSpeed * Time.deltaTime;

        if (yAng > yAngMax) yAng = yAngMax;
        if (yAng < yAngMin) yAng = yAngMin;
        if (xAng > xAngMax) xAng = xAngMax;
        if (xAng < xAngMin) xAng = xAngMin;

        float ay = 0f; // angle of segment around Y-axis
        float ax = 0f; //    ''                   X-axis
        Vector3 p = ((float)(Snake.SegmentsCount-SegmentSep) - 0.5f) * Snake.SegmentsDist * Vector3.forward; // position of segment's edge
        float dax = xAng / (float) SegmentSep;
        float day = yAng / (float) SegmentSep;
        float c = Math.Max(0f, 1f - Mathf.Abs(xAng / xAngStraight));

        // Reshape forward part of the snake
        for (int i = SegmentSep - 1; i >= 0; --i) {
            ay += c * jointAngs[i] + day;
            ax += dax;
            Quaternion r = Quaternion.AngleAxis(ax, Vector3.right);
            backs[i].localRotation = r * Quaternion.AngleAxis(ay, Vector3.up);
            Vector3 f = backs[i].localRotation * Vector3.forward;
            backs[i].localPosition = p + 0.5f * Snake.SegmentsDist * f;
            p += Snake.SegmentsDist * f;
        }
    }
}
