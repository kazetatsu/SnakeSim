using UnityEngine;

public class Belly : MonoBehaviour
{
    HingeJoint joint;
    void Start() { joint = GetComponent<HingeJoint>(); }
    void FixedUpdate() { joint.useMotor = (joint.velocity <= -0.01f); }
}
