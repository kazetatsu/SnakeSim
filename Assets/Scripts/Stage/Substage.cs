// SPDX-FileCopyrightText: 2025 Shinagwa Kazemaru
// SPDX-License-Identifier: BSD-2-Clause license

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
    SimpleTrigger trigger;
    [SerializeField] AudioClip music;
    [SerializeField] GameObject[] omittableObjs;


    public void Prioritize() {
        _camera.Prioritize();
        switcher.Switch(music);
        rotater.RotateSun(index);
        foreach (GameObject obj in omittableObjs)
            obj.SetActive(true);
    }


    public void Deprioritize() {
        foreach (GameObject obj in omittableObjs)
            obj.SetActive(false);
    }


    public float GetDistanceFromSnake() {
        if (trigger is not null && trigger.IsSnakeInside())
            return 0f;
        return (ankerPos - Snake.headPos).magnitude;
    }


    void Start() {
        ankerPos = transform.GetChild(0).transform.position;
        trigger = GetComponentInChildren<SimpleTrigger>();
        _camera = GetComponentInChildren<CinemachineCamera>();
        switcher = GameObject.Find("MusicSource").GetComponent<MusicSwitcher>();
        rotater = GameObject.Find("Sun").GetComponent<SunRotater>();
    }
}
