#if UNITY_EDITOR

using UnityEditor;

public class RefreshOnPlay {
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
