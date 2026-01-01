// SPDX-FileCopyrightText: 2025 Shinagwa Kazemaru
// SPDX-License-Identifier: BSD-2-Clause license

using UnityEngine;

public class FireWork : MonoBehaviour
{
    [SerializeField] Color[] colors;
    [SerializeField] float delay;
    [SerializeField] float interval;
    float timer;

    ParticleSystem particle;


    void Start() {
        particle = GetComponent<ParticleSystem>();
        timer = delay;
    }


    void Update() {
        if (timer < interval) {
            timer += Time.deltaTime;
        } else {
            var temp  = particle.main;
            temp.startColor = colors[Random.Range(0,colors.Length)];
            particle.Play();
            timer = 0f;
        }
    }
}
