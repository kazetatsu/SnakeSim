using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FirstButtonSelecter : MonoBehaviour
{
    void Start() {
        Button buttonContinue = GameObject.Find("Continue").GetComponent<Button>();

        if (File.Exists(SaveDataIO.GetPath())) {
            buttonContinue.interactable = true;
            EventSystem.current.SetSelectedGameObject(buttonContinue.gameObject);
        } else {
            buttonContinue.interactable = false;
            EventSystem.current.SetSelectedGameObject(GameObject.Find("New Game"));
        }
    }


    void Update() {}
}
