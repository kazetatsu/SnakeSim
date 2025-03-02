using System.Collections.Generic;
using UnityEngine;

public class Vis : MonoBehaviour
{
    JointsManager _manager;
    List<Material> _materials;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _manager = GetComponent<JointsManager>();
        _materials = new List<Material>();

        for (int i = 1; i <= _manager.JointsCount; ++i) {
            Transform child = this.transform.Find("Segment " + i.ToString("d2"));

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
        for (int i = 0; i < _manager.JointsCount; ++i) {
            float ang = _manager.targetRotations[i].eulerAngles.x;
            float r = Mathf.Min(ang, 360f-ang) / 60f;
            _materials[i].color = new Color(r, 0f, 0f);
        }
    }
}
