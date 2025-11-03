using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] int spawnPointID;
    Moderator moderator;


    void Start() {
        moderator = GameObject.Find("Moderator").GetComponent<Moderator>();
    }


    void OnTriggerEnter(Collider collider) {
        moderator.TrySetSpawnPointID(spawnPointID);
        Destroy(this);
    }
}
