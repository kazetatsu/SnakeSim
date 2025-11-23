using UnityEngine;

public class Moderator : MonoBehaviour
{
    SaveData data;
    // Spawn point is a check point s.t. the snake spawn.
    public int SpawnPointID { get => data.spawnPointID; }


    public bool TrySetSpawnPointID(int ID) {
        if (data.spawnPointID < ID) {
            data.spawnPointID = ID;
            SaveDataIO.Write(data);
            return true;
        }
        return false;
    }


    void Start() {
        data = SaveDataIO.Read() ?? new SaveData();
    }


    void Update() {

    }
}
