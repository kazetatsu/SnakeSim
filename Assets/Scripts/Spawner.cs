using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject bonePrefab;
    [SerializeField] GameObject segmentPrefab;
    Skin skin;
    Transform bone;
    [SerializeField] List<Transform> spawnPoints;
    int spawnPointID = 0;


    public bool TrySetSpawnPointID(int ID) {
        if (0 <= ID && ID < spawnPoints.Count) {
            spawnPointID = ID;
            return true;
        } else return false;
    }


    void Spawn(Vector3 position, Quaternion rotation) {
        if (bone) Destroy(bone.gameObject);

        bone = Instantiate(bonePrefab).transform;
        bone.position = position;
        bone.rotation = rotation;

        Transform tail = bone.GetChild(2);
        int count = Snake.SegmentsCount - 2; // number of middle segments

        for (int i = 2; i <= count; ++i) {
            var segment = Instantiate(segmentPrefab, bone).transform;
            segment.SetSiblingIndex(i);
            segment.name = "Segment " + i.ToString("d2");
            segment.localPosition = (float)i * Snake.SegmentsDist * Vector3.back;
        }

        tail.localPosition = (float)(count + 1) * Snake.SegmentsDist * Vector3.back;

        skin.SetBone(bone);
    }


    void Spawn() {
        Spawn(spawnPoints[spawnPointID].position, spawnPoints[spawnPointID].rotation);
    }


    void Start() {
        skin = GameObject.Find("SnakeSkin").GetComponent<Skin>();
        Spawn();
    }


    // for debug
    public void ForceSpawn(int ID) {
        Spawn(spawnPoints[ID].position, spawnPoints[ID].rotation);
    }
}
