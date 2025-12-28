using UnityEngine;
using UnityEngine.InputSystem;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject bonePrefab;
    Skin skin;
    Transform bone;
    [SerializeField] Transform[] checkPoints; // transforms of all instances of class:CheckPoint
    int spawnIndex = 0;
    InputAction action;

    [SerializeField] float intervalSpawn;
    float timer = 0f;
    [SerializeField] bool useSavedata = true;


    public int GetCheckPointID(Transform t) {
        for (int i = 0; i < checkPoints.Length; ++i)
            if (checkPoints[i] == t)
                return i;
        return -1;
    }


    // Spawn point is a check point s.t. the snake spawn.
    public void SetSpawnPoint(int checkPointID) { spawnIndex = checkPointID; }


    void Spawn(Vector3 position, Quaternion rotation) {
        if (timer > 0f) return;
        if (bone) Destroy(bone.gameObject);

        bone = Instantiate(bonePrefab).transform;
        bone.position = position;
        bone.rotation = rotation;

        skin.SetBone(bone);
        timer = intervalSpawn;
    }


    void Spawn() {
        Spawn(checkPoints[spawnIndex].position, checkPoints[spawnIndex].rotation);
    }


    void Start() {
        skin = GameObject.Find("SnakeSkin").GetComponent<Skin>();
        if (useSavedata)
            spawnIndex = SaveData.SpawnPointID;
        else
            spawnIndex = 0;
        action = InputSystem.actions.FindAction("Spawn");
        Spawn();
    }


    void Update() {
        if (timer > 0f)
            timer -= Time.deltaTime;

        if (Time.timeScale > 0f && action.IsPressed())
            Spawn();
    }


    // for debug
    public void ForceSpawn(int index) {
        Spawn(checkPoints[index].position, checkPoints[index].rotation);
    }
}
