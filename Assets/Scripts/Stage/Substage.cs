using Unity.Cinemachine;
using UnityEngine;

// Manage camera & music
public class Substage : MonoBehaviour
{
    CinemachineCamera _camera;
    MusicSwitcher switcher;
    Vector3 ankerPos;
    public Vector3 AnkerPosition { get => ankerPos; }
    [SerializeField] AudioClip music;

    public void Prioritize() {
        _camera.Prioritize();
        switcher.Switch(music);
    }


    void Start() {
        ankerPos = transform.GetChild(0).transform.position;
        _camera = GetComponentInChildren<CinemachineCamera>();
        switcher = GameObject.Find("Audio Source").GetComponent<MusicSwitcher>();
    }


    void Update() {

    }
}
