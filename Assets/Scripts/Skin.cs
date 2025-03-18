using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class Skin : MonoBehaviour
{
    Mesh _mesh;

    [SerializeField] float dist;
    [SerializeField] float radius;
    int segmentsCount;

    [SerializeField] Transform physicalBody;
    Transform[] backs;

    int vertsCount;
    Vector3[] localVerts;
    int[][] localFVertIndexs;
    int[][] localBVertIndexs;


    class SlerpHelper {
        float[] to;
        float theta;
        bool tooClose = false;

        public SlerpHelper(Quaternion to) {
            if (to.w >= 0f) {
                this.to = new float[4]{ to.w,  to.x,  to.y,  to.z};
            } else {
                this.to = new float[4]{-to.w, -to.x, -to.y, -to.z};
            }

            theta = Mathf.Acos(to.w);

            if (theta <= 0.001f) {
                tooClose = true;
            } else {
                float invSin = 1f / Mathf.Sin(theta);
                for (int i = 0; i < 4; ++i) {
                    this.to[i] *= invSin;
                }
            }
        }

        public Quaternion Slerp(float t) {
            if (tooClose) {
                return new Quaternion(to[1], to[2], to[3], to[0]);
            } else {
                float coefF = Mathf.Sin((1f-t) * theta);
                float coefT = Mathf.Sin(t * theta);
                return new Quaternion(
                    coefT * to[1],
                    coefT * to[2],
                    coefT * to[3],
                    coefT * to[0] + coefF
                );
            }
        }
    }
    Quaternion GetHalfRotation (Quaternion rot) {
        float[] q;
        if (rot.w >= 0f) {
            q = new float[4]{1f + rot.w,  rot.x,  rot.y,  rot.z};
        } else {
            q = new float[4]{1f - rot.w, -rot.x, -rot.y, -rot.z};
        }
        float norm = 0f;
        for (int i = 0; i < 4; ++i)
            norm += q[i] * q[i];
        norm = Mathf.Sqrt(norm);
        return new Quaternion(q[1]/norm, q[2]/norm, q[3]/norm, q[0]/norm);
    }

    void Start() {
        segmentsCount = physicalBody.childCount;

        _mesh = this.GetComponent<MeshFilter>().mesh;

        Vector3[] verts = _mesh.vertices;

        localVerts = new Vector3[verts.Length];
        var localFVertIndexs = new List<int>[segmentsCount];
        var localBVertIndexs = new List<int>[segmentsCount];
        for (int i = 0; i < segmentsCount; ++i) {
            localFVertIndexs[i] = new List<int>();
            localBVertIndexs[i] = new List<int>();
        }

        vertsCount = verts.Length;
        for (int k = 0; k < vertsCount; ++k) {
            float zd = -verts[k].z / dist;
            int i = Mathf.FloorToInt(zd);
            if (i < 0) i = 0;
            if (i >= segmentsCount) i = segmentsCount - 1;

            localVerts[k] = verts[k] + ((float)i + 0.5f) * dist * Vector3.forward;

            if (zd - (float)i <= 0.5f) {
                localFVertIndexs[i].Add(k);
            } else {
                localBVertIndexs[i].Add(k);
            }
        }

        this.localFVertIndexs = new int[segmentsCount][];
        this.localBVertIndexs = new int[segmentsCount][];
        for (int i = 0; i < segmentsCount; ++i) {
            this.localFVertIndexs[i] = localFVertIndexs[i].ToArray();
            this.localBVertIndexs[i] = localBVertIndexs[i].ToArray();
        }

        backs = new Transform[segmentsCount];
        for (int i = 0; i < segmentsCount; ++i) {
            backs[i] = physicalBody.GetChild(i).Find("Back");
        }

        // foreach (int k in this.localFVertIndexs[0])
        //     verts[k].y += 1f;
        // _mesh.SetVertices(verts);
        // _mesh.RecalculateNormals();
        // _mesh.RecalculateBounds();
    }


    void Update() {
        var verts = new Vector3[vertsCount];

        Vector3 right = backs[0].right;
        Vector3 up = backs[0].up;
        Vector3 forward = backs[0].forward;
        Vector3 center = backs[0].position;
        float disth = 0.5f * dist;
        float aspect = radius / disth;
        float invRadius = 1f/radius;

        foreach (int k in localFVertIndexs[0]) {
            Vector3 localVert = localVerts[k];
            verts[k] = localVert.x * right + localVert.y * up + localVert.z * forward + center;
        }

        for (int i = 0; i < segmentsCount-1; ++i) {
            var roth = GetHalfRotation(Quaternion.Inverse(backs[i].rotation) * backs[i+1].rotation);
            var helper = new SlerpHelper(roth);

            foreach (int k in localBVertIndexs[i]) {
                float t = -localVerts[k].z / disth;
                Quaternion rott = helper.Slerp(t);
                right = rott * Vector3.right;
                up = rott * Vector3.up;
                Vector3 rotatedXY = localVerts[k].x * right + localVerts[k].y * up;
                Vector3 direction = rotatedXY.normalized;
                float xy2 = direction.x * direction.x + direction.y * direction.y;

                Vector3 localVert;
                if (-t + aspect * direction.z / Mathf.Sqrt(xy2) >= -1f) {
                    localVert = rotatedXY * Mathf.Sqrt(1f + direction.z * direction.z / xy2);
                } else {
                    float zdh = localVerts[k].z + disth;
                    float a = -direction.z * zdh + Mathf.Sqrt(-xy2 * zdh * zdh + radius * radius);
                    localVert = a * invRadius * rotatedXY;
                }
                localVert.z += localVerts[k].z;

                verts[k] = backs[i].rotation * localVert + backs[i].position;
            }
        }

        for (int i = 1; i < segmentsCount; ++i) {
            var roth = GetHalfRotation(Quaternion.Inverse(backs[i].rotation) * backs[i-1].rotation);
            var helper = new SlerpHelper(roth);

            foreach (int k in localFVertIndexs[i]) {
                float t = localVerts[k].z / disth;
                Quaternion rott = helper.Slerp(t);
                right = rott * Vector3.right;
                up = rott * Vector3.up;
                Vector3 rotatedXY = localVerts[k].x * right + localVerts[k].y * up;
                Vector3 direction = rotatedXY.normalized;
                float xy2 = direction.x * direction.x + direction.y * direction.y;

                Vector3 localVert;
                if (t + aspect * direction.z / Mathf.Sqrt(xy2) <= 1f) {
                    localVert = rotatedXY * Mathf.Sqrt(1f + direction.z * direction.z / xy2);
                } else {
                    float zdh = localVerts[k].z - disth;
                    float a = -direction.z * zdh + Mathf.Sqrt(-xy2 * zdh * zdh + radius * radius);
                    localVert = a * invRadius * rotatedXY;
                }
                localVert.z += localVerts[k].z;

                verts[k] = backs[i].rotation * localVert + backs[i].position;
            }
        }

        Quaternion rot = backs[segmentsCount-1].rotation;
        center = backs[segmentsCount-1].position;
        foreach (int k in localBVertIndexs[segmentsCount-1]) {
            verts[k] = rot * localVerts[k] + center;
        }

        _mesh.SetVertices(verts);
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();
    }
}
