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
    Vector3[][] localFVerts; // forward vertices
    int[][] localFVertIndexs;
    Vector3[][] localBVerts; // backward vertices
    int[][] localBVertIndexs;


    class SlerpHelper {
        float[] from;
        float[] to;
        float theta;
        bool tooClose = false;

        public SlerpHelper(Quaternion from, Quaternion to) {
            this.from = new float[4]{from.w, from.x, from.y, from.z};
            this.to = new float[4]{to.w, to.x, to.y, to.z};

            float cos = 0f;
            for (int i = 0; i < 4; ++i) 
                cos += this.from[i] * this.to[i];
            if (cos < 0f) {
                for (int i = 0; i < 4; ++i) 
                    this.to[i] = - this.to[i];
                cos = -cos;
            }

            theta = Mathf.Acos(cos);

            if (theta <= 0.001f) {
                tooClose = true;
            } else {
                float invSin = 1f / Mathf.Sin(theta);
                for (int i = 0; i < 4; ++i) {
                    this.from[i] *= invSin;
                    this.to[i] *= invSin;
                }
            }
        }

        public Quaternion Slerp(float t) {
            if (tooClose) {
                return new Quaternion(from[1], from[2], from[3], from[0]);
            } else {
                float coefF = Mathf.Sin((1f-t) * theta);
                float coefT = Mathf.Sin(t * theta);
                return new Quaternion(
                    coefF * from[1] + coefT * to[1],
                    coefF * from[2] + coefT * to[2],
                    coefF * from[3] + coefT * to[3],
                    coefF * from[0] + coefT * to[0]
                );
            }
        }
    }


    void Start() {
        segmentsCount = physicalBody.childCount;

        _mesh = this.GetComponent<MeshFilter>().mesh;

        Vector3[] verts = _mesh.vertices;

        var localFVerts = new List<Vector3>[segmentsCount];
        var localFVertIndexs = new List<int>[segmentsCount];
        var localBVerts = new List<Vector3>[segmentsCount];
        var localBVertIndexs = new List<int>[segmentsCount];
        for (int i = 0; i < segmentsCount; ++i) {
            localFVerts[i] = new List<Vector3>();
            localFVertIndexs[i] = new List<int>();
            localBVerts[i] = new List<Vector3>();
            localBVertIndexs[i] = new List<int>();
        }

        vertsCount = verts.Length;
        for (int k = 0; k < vertsCount; ++k) {
            float zd = -verts[k].z / dist;
            int i = Mathf.FloorToInt(zd);
            if (i < 0) i = 0;
            if (i >= segmentsCount) i = segmentsCount - 1;

            Vector3 localVert = verts[k] + ((float)i + 0.5f) * dist * Vector3.forward;
            if (zd - (float)i <= 0.5f) {
                localFVerts[i].Add(localVert);
                localFVertIndexs[i].Add(k);
            } else {
                localBVerts[i].Add(localVert);
                localBVertIndexs[i].Add(k);
            }
        }

        this.localFVerts = new Vector3[segmentsCount][];
        this.localFVertIndexs = new int[segmentsCount][];
        this.localBVerts = new Vector3[segmentsCount][];
        this.localBVertIndexs = new int[segmentsCount][];
        for (int i = 0; i < segmentsCount; ++i) {
            this.localFVerts[i] = localFVerts[i].ToArray();
            this.localFVertIndexs[i] = localFVertIndexs[i].ToArray();
            this.localBVerts[i] = localFVerts[i].ToArray();
            this.localBVertIndexs[i] = localFVertIndexs[i].ToArray();
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
        var verts = _mesh.vertices;

        int n = localFVerts[0].Length;
        Vector3 right = backs[0].right;
        Vector3 up = backs[0].up;
        Vector3 forward = backs[0].forward;
        Vector3 center = backs[0].position;
        for (int s = 0; s < n; ++s) {
            int k = localFVertIndexs[0][s];
            Vector3 localVert = localFVerts[0][s];
            verts[k] = localVert.x * right + localVert.y * up + localVert.z * forward + center;
        }

        _mesh.SetVertices(verts);
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();
    }
}
