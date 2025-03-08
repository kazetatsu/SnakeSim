using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class Skin : MonoBehaviour
{
    Mesh _mesh;
    const int vertCount = 9;
    const int LoopCut = 3;
    float[] xs;
    float[] ys;
    Vector3[] verts;

    Transform[] backs;
    [SerializeField] float dist;
    [SerializeField] float radius;
    int segmentsCount;

    void Start() {
        _mesh = new Mesh();
        this.GetComponent<MeshFilter>().mesh = _mesh;

        segmentsCount = this.transform.childCount;
        backs = new Transform[segmentsCount];
        for (int i = 0; i < segmentsCount; ++i)
            backs[i] = this.transform.GetChild(i).Find("Back");

        xs = new float[vertCount];
        ys = new float[vertCount];
        for (int i = 0; i < vertCount; ++i) {
            float theta = 2f * Mathf.PI * (float)i / (float)vertCount;
            ys[i] = radius * Mathf.Cos(theta);
            xs[i] = radius * Mathf.Sin(theta);
        }
        verts = new Vector3[vertCount * (LoopCut * segmentsCount + 1)];
        for (int i = 0; i < vertCount; ++i) {
            var vert = new Vector3(xs[i], ys[i], 0f);
            for (int j = 0; j < segmentsCount; ++j) {
                int offset = (LoopCut*vertCount)*j + i;

                vert.z = - (float)j * dist + 0.5f * dist;
                verts[offset              ] = vert;
                vert.z -= 0.2f * dist;
                verts[offset + vertCount  ] = vert;
                vert.z -= 0.6f * dist;
                verts[offset + vertCount*2] = vert;
            }
            vert.z = - (float)segmentsCount * dist;
            verts[(LoopCut * vertCount) * segmentsCount + i] = vert;
        }

        var tris = new int[3 * segmentsCount * (LoopCut * 2 * vertCount)];
        for (int j = 0; j < segmentsCount; ++j) {
            for (int k = 0; k < LoopCut; ++k) {
                int offsetVert = (vertCount*LoopCut) * j + vertCount * k;
                for (int i = 0; i < vertCount; ++i) {
                    int offsetTri = (3*2*vertCount*LoopCut) * j + (3*2*vertCount) * k + 3*2*i;

                    tris[offsetTri    ] = offsetVert + i;
                    tris[offsetTri + 1] = offsetVert + (i+1)%vertCount;
                    tris[offsetTri + 2] = offsetVert + vertCount + i;

                    tris[offsetTri + 3] = offsetVert + vertCount + i;
                    tris[offsetTri + 4] = offsetVert + (i+1)%vertCount;
                    tris[offsetTri + 5] = offsetVert + vertCount + (i+1)%vertCount;
                }
            }
        }

        var uvs = new Vector2[vertCount * (LoopCut * segmentsCount + 1)];

        _mesh.SetVertices(verts);
        _mesh.SetTriangles(tris, 0);
        _mesh.SetUVs(0, uvs);
        _mesh.RecalculateBounds();
        _mesh.RecalculateNormals();
    }


    void Update() {
        // --------------------------------
        //  Middle part of each segment
        // --------------------------------
        for (int j = 0; j < segmentsCount; ++j) {
            for (int i = 0; i < vertCount; ++i) {
                int offset = (LoopCut*vertCount)*j + i;
                Vector3 vertXY = xs[i] * backs[j].right + ys[i] * backs[j].up + backs[j].position;

                verts[offset + vertCount  ] = vertXY + 0.3f * dist * backs[j].forward;
                verts[offset + vertCount*2] = vertXY - 0.3f * dist * backs[j].forward;
            }
        }

        // --------------------------------
        //  Edge
        // --------------------------------
        Transform back = backs[0];
        Vector3 center = back.position + 0.5f * dist * back.forward;
        for (int i = 0; i < vertCount; ++i) {
            verts[i] = xs[i] * back.right + ys[i] * back.up + center;
        }

        back = backs[segmentsCount - 1];
        center = back.position - 0.5f * dist * back.forward;
        for (int i = 0; i < vertCount; ++i) {
            verts[(LoopCut*vertCount)*segmentsCount + i] = xs[i] * back.right + ys[i] * back.up + center;
        }

        // --------------------------------
        //  Joint
        // --------------------------------
        for (int j = 1; j < segmentsCount; ++j) {
            Vector3 forward = (backs[j-1].forward + backs[j].forward).normalized;
            var up = (backs[j-1].up + backs[j].up).normalized;
            var right = Vector3.Cross(up, forward);
            center = backs[j].position + 0.5f * dist * backs[j].forward;
            var shorterAxis = Vector3.Cross(backs[j].forward, backs[j-1].forward).normalized; // Ideal length is 1.
            if (shorterAxis.magnitude < 0.5f) {
                for (int i = 0; i < vertCount; ++i) {
                    verts[(LoopCut*vertCount)*j + i] = xs[i] * right + ys[i] * up + center;
                }
            } else {
                var longerAxis = Vector3.Cross(shorterAxis, forward);
                float cos = Vector3.Dot(forward, backs[j].forward);

                for  (int i = 0; i < vertCount; ++i) {
                    Vector3 xy = xs[i] * right + ys[i] * up;
                    float dot = Vector3.Dot(xy, longerAxis);
                    if (dot > 0f) {
                        xy += (- 1f + 1f / cos) * dot * longerAxis;
                    }
                    verts[(LoopCut*vertCount)*j + i] = xy + center;
                }
            }
        }

        _mesh.SetVertices(verts);
        _mesh.RecalculateBounds();
        _mesh.RecalculateNormals();
    }
}
