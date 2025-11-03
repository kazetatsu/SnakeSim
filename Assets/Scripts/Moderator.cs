using UnityEngine;

public class Moderator : MonoBehaviour
{
    SaverLoader sl;
    Spawner spawner;


    public bool TrySetSpawnPointID(int ID) {
        if (sl.data.spawnPointID < ID && spawner.TrySetSpawnPointID(ID)) {
            sl.data.spawnPointID = ID;
            sl.Save();
            return true;
        } else return false;
    }


    void Start() {
        sl = new SaverLoader();
        sl.Load();

        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        spawner.TrySetSpawnPointID(sl.data.spawnPointID);
    }


    void Update() {

    }
}
