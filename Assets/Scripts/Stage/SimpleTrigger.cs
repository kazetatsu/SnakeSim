using UnityEngine;

public class SimpleTrigger : MonoBehaviour
{
    [SerializeField] Vector3 size;


    void OnDrawGizmosSelected() {
        var points = new Vector3[4];
        Vector3 x = size.x * transform.right;
        Vector3 y = size.y * transform.up;
        Vector3 z = size.z * transform.forward;
        points[0] = transform.position - z + x + y;
        points[1] = transform.position - z + x - y;
        points[2] = transform.position - z - x - y;
        points[3] = transform.position - z - x + y;
        Gizmos.DrawLineStrip(points, true);

        points[0] = transform.position + z + x + y;
        points[1] = transform.position + z + x - y;
        points[2] = transform.position + z - x - y;
        points[3] = transform.position + z - x + y;
        Gizmos.DrawLineStrip(points, true);

        Gizmos.DrawLine(transform.position + z + x + y, transform.position - z + x + y);
        Gizmos.DrawLine(transform.position + z + x - y, transform.position - z + x - y);
        Gizmos.DrawLine(transform.position + z - x - y, transform.position - z - x - y);
        Gizmos.DrawLine(transform.position + z - x + y, transform.position - z - x + y);
    }


    public bool IsSnakeInside() {
        Vector3 p = transform.InverseTransformPoint(Snake.headPos);
        return (
            -size.x <= p.x && p.x <= size.x &&
            -size.y <= p.y && p.y <= size.y &&
            -size.z <= p.z && p.z <= size.z
        );
    }
}
