using UnityEngine;

public class SubstageArea : MonoBehaviour
{
    const float DisableDist = .5f;
    const float FadeDist = 3f;

    [SerializeField] Vector3 size;
    public bool isInside;
    public float fade = 1f;
    public AudioClip musicClip;


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


    void Update() {
        Vector3 p = transform.InverseTransformPoint(Snake.headPos);
        bool _isInside = true;
        float distMin = float.PositiveInfinity;
        // Signed distance from snake's position to each surface of area
        var dists = new float[6] {
            size.x - p.x, size.x + p.x,
            size.y - p.y, size.y + p.y,
            size.z - p.z, size.z + p.z
        };

        // Check is the snake inside of this area
        // & distance from the snake to this area
        for (int i = 0; i < 6; ++i) {
            if (dists[i] < 0f) {
                _isInside = false;
                dists[i] = -dists[i];
            }
            if (dists[i] < distMin)
                distMin = dists[i];
        }

        if (_isInside) {
            isInside = true;
            fade = Mathf.Min(1f, distMin / FadeDist);
        }
        else if (distMin > DisableDist) // Outside and far
            isInside = false;
    }
}
