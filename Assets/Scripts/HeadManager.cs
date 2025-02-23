using UnityEngine;

public class HeadManager : MonoBehaviour
{
    public const int SegmentsCount = 3;
    private ConfigurableJoint joint1;
    private ConfigurableJoint joint2;

    public float angLift;
    public float angYaw;

    void Start() {
        var backs = new Transform[3];
        for (int i = 0; i < SegmentsCount; ++i)
            backs[i] = this.transform.Find("Head" + i.ToString("d2")).Find("Back");

        joint1 = backs[1].GetComponent<ConfigurableJoint>();
        joint2 = backs[2].GetComponent<ConfigurableJoint>();

        joint1.connectedBody = backs[0].GetComponent<Rigidbody>();
        joint2.connectedBody = backs[1].GetComponent<Rigidbody>();

        Physics.IgnoreCollision(
            backs[0].GetComponentInChildren<Collider>(),
            backs[1].GetComponentInChildren<Collider>()
        );
        Physics.IgnoreCollision(
            backs[1].GetComponentInChildren<Collider>(),
            backs[2].GetComponentInChildren<Collider>()
        );
    }

    void FixedUpdate() {
        joint1.targetRotation = Quaternion.Euler(angYaw, -angLift, 0f);
        joint2.targetRotation = Quaternion.Euler(angYaw, -angLift, 0f);
    }
}
