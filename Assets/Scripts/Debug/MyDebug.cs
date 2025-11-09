using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR

using UnityEditor;

public class RefreshPlay {
    [InitializeOnLoadMethod]
    public static void Run() {
        EditorApplication.playModeStateChanged += (PlayModeStateChange state) => {
            if (state == PlayModeStateChange.ExitingEditMode) {
                AssetDatabase.Refresh();
            }
        };
    }
}

#endif

public class MyDebug : MonoBehaviour
{
    int n;
    List<InputAction> actions;

    Spawner spawner;


    void Start() {
        actions = new List<InputAction>();
        int i = 0;
        while (true) {
            var action = InputSystem.actions.FindAction("Spawn" + i.ToString());
            if (action is null) break;
            actions.Add(action);
            ++i;
        }
        n = actions.Count;

        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
    }


    void Update() {
        for (int i = 0; i < n; ++i) {
            if (actions[i].IsPressed()) {
                spawner.ForceSpawn(i);
                break;
            }
        }
    }
}
