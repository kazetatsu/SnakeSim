using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageLoader : MonoBehaviour
{
    [SerializeField] string fileName;


    public void Load() {
        SceneManager.LoadScene("Stage");
    }


    void Start() {
        SaveDataIO.fileName = fileName;
    }


    void Update() {}
}
