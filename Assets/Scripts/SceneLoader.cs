using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void Quit() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }


    public void LoadWithCurtain(float delay, string sceneName) {
        GameObject.Find("Curtain")?.GetComponent<CurtainOpener>()?.Close(
            delay,
            () => {SceneManager.LoadScene(sceneName);}
        );
    }


    public void ToStageNewGame() {
        SaveData.SpawnPointID = 0;
        LoadWithCurtain(0f, "Stage");
    }


    public void ToStageContinue() {
        LoadWithCurtain(0f, "Stage");
    }


    public void ToTitle() {
        LoadWithCurtain(0f, "Title");
    }
}
