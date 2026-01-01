// SPDX-FileCopyrightText: 2025 Shinagwa Kazemaru
// SPDX-License-Identifier: BSD-2-Clause license

using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    Spawner spawner;
    int ID;
    CheckerFlag flag;
    Collider _collider;


    void Start() {
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        ID = spawner.GetCheckPointID(transform); // Get own checkpointID
        flag = transform.GetComponentInChildren<CheckerFlag>();
        _collider = GetComponent<Collider>();

        if (ID <= SaveData.SpawnPointID) {
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
        if (SaveData.SpawnPointID < ID) {
            SaveData.SpawnPointID = ID;
            spawner.SetSpawnPoint(ID);
            flag.RaiseFlagGradually();
        }
        Destroy(_collider);
        Destroy(this);
    }
}
