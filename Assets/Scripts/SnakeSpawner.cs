using UnityEngine;
using UnityEngine.InputSystem;

public class SnakeSpawner : MonoBehaviour
{
    [SerializeField] GameObject bonePrefab;
    [SerializeField] GameObject segmentPrefab;
    Skin skin;
    Transform bone;

    void Start()
    {
        skin = GameObject.Find("SnakeSkin").GetComponent<Skin>();
    }

    public void Spawn(Vector3 position, Quaternion rotation)
    {
        if (bone) Destroy(bone.gameObject);

        bone = Instantiate(bonePrefab).transform;
        bone.position = position;
        bone.rotation = rotation;

        Transform tail = bone.GetChild(2);
        int count = Consts.SegmentsCount - 2; // number of middle segments

        for (int i = 2; i <= count; ++i)
        {
            var segment = Instantiate(segmentPrefab, bone).transform;
            segment.SetSiblingIndex(i);
            segment.name = "Segment " + i.ToString("d2");
            segment.localPosition = (float)i * Consts.SegmentsDist * Vector3.back;
        }

        tail.localPosition = (float)(count + 1) * Consts.SegmentsDist * Vector3.back;

        skin.SetBone(bone);
    }

    // for debug
    public void OnPressed(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        Spawn(new Vector3(0f, 10f, 0f), Quaternion.identity);
    }
}
