using UnityEngine;

public class Moderator : MonoBehaviour
{
    SaverLoader sl;
    // Spawn point is a check point s.t. the snake spawn.
    public int SpawnPointID { get => sl.data.spawnPointID; }


    public bool TrySetSpawnPointID(int ID) {
        if (sl.data.spawnPointID < ID) {
            sl.data.spawnPointID = ID;
            sl.Save();
            return true;
        }
        return false;
    }


    void Start() {
        sl = new SaverLoader();
        sl.Load();
    }


    void Update() {

    }
}
