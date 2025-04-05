using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class JointsManager : MonoBehaviour
{
    int jointsCount;
    public int JointsCount { get => jointsCount; }

    ConfigurableJoint[] joints;
    public Quaternion[] targetRotations;

    void Start() {
        // Get ConfigurableJoint of each segment
        // & Connect bodies  between connected segments
        // & Avoid collision "
        var _joints = new List<ConfigurableJoint>();
        int childsCount = this.transform.childCount;
        Transform back = this.transform.GetChild(0).Find("Back");
        var frontBody = back.GetComponent<Rigidbody>();
        var frontCollider = back.GetComponentInChildren<Collider>();

        for (int i = 1; i < childsCount; ++i) {
            back = this.transform.GetChild(i).Find("Back");

            //Get Conf...
            var joint = back.GetComponent<ConfigurableJoint>();
            _joints.Add(joint);

            // Connect bodies
            joint.connectedBody = frontBody;
            frontBody = back.GetComponent<Rigidbody>();

            // Avoid collision
            var _collider = back.GetComponentInChildren<Collider>();
            Physics.IgnoreCollision(frontCollider, _collider);
            frontCollider = _collider;
        }

        // Init member variables
        jointsCount = _joints.Count;
        joints = _joints.ToArray();
        targetRotations = new Quaternion[jointsCount];
        for (int i = 0; i < jointsCount; ++i)
            targetRotations[i] = Quaternion.identity;
    }

    void FixedUpdate() {
        for (int i = 0; i < jointsCount; ++i) {
            Quaternion rot = targetRotations[i];
            if (   (float.IsNormal(rot.w) || rot.w == 0f)
                && (float.IsNormal(rot.x) || rot.x == 0f)
                && (float.IsNormal(rot.y) || rot.y == 0f)
                && (float.IsNormal(rot.z) || rot.z == 0f)
            ) {
                joints[i].targetRotation = rot;
            }
        }
    }
}
