using System.Collections.Generic;
using UnityEngine;

public class Vis : MonoBehaviour
{
    SegmentsManager _manager;
    List<Material> _materials;
    int jointCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _manager = GetComponent<SegmentsManager>();
        _materials = new List<Material>();
        jointCount = _manager.MiddlesCount;

        for (int i = 1; i <= jointCount; ++i) {
            Transform child = this.transform.Find("Middle" + i.ToString("d2"));

            _materials.Add(
                child
                .Find("Back")
                .Find("Collider")
                .GetComponent<MeshRenderer>()
                .material
            );
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < jointCount; ++i) {
            float ang = _manager.middleJointAngles[i];
            ang /=60f;
            if (ang < 0f)  ang = -ang;
            if (ang >= 1f) ang = 1f;
            _materials[i].color = new Color(ang,0f,0f);
        }
    }
}
