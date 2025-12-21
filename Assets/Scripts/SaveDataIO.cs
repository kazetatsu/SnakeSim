using UnityEngine;

public static class SaveData {
    public static int SpawnPointID {
        get => PlayerPrefs.GetInt("SpawnPointID", 0);
        set => PlayerPrefs.SetInt("SpawnPointID", value);
    }

    public static bool HasProgress() {
        return SpawnPointID > 0;
    }
}
