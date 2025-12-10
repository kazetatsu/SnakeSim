using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    Moderator moderator;
    Spawner spawner;
    int ID;
    CheckerFlag flag;
    Collider _collider;


    void Start() {
        moderator = GameObject.Find("Moderator").GetComponent<Moderator>();
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        ID = spawner.GetCheckPointID(transform); // Get own checkpointID
        flag = transform.GetComponentInChildren<CheckerFlag>();
        _collider = GetComponent<Collider>();

        if (ID <= moderator.SpawnPointID) {
            flag?.RaiseFlagImmediately();
            Transform confetti = transform.Find("Confetti");
            if (confetti is not null)
                Destroy(confetti.gameObject);
            Destroy(_collider);
            Destroy(this);
        }
    }


    void OnTriggerEnter(Collider _) {
        transform.Find("Confetti")?.GetComponent<ParticleSystem>().Play();
        if (moderator.TrySetSpawnPointID(ID)) {
            spawner.SetSpawnPoint(ID);
            flag.RaiseFlagGradually();
        }
        Destroy(_collider);
        Destroy(this);
    }
}
