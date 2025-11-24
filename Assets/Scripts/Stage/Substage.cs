using Unity.Cinemachine;
using UnityEngine;

// Manage camera & music
public class Substage : MonoBehaviour
{
    [SerializeField] int index;
    CinemachineCamera _camera;
    MusicSwitcher switcher;
    SunRotater rotater;

    Vector3 ankerPos;
    public Vector3 AnkerPosition { get => ankerPos; }
    [SerializeField] AudioClip music;

    public void Prioritize() {
        _camera.Prioritize();
        switcher.Switch(music);
        rotater.RotateSun(index);
    }


    void Start() {
        ankerPos = transform.GetChild(0).transform.position;
        _camera = GetComponentInChildren<CinemachineCamera>();
        switcher = GameObject.Find("Audio Source").GetComponent<MusicSwitcher>();
        rotater = GameObject.Find("Sun").GetComponent<SunRotater>();
    }


    void Update() {

    }
}
