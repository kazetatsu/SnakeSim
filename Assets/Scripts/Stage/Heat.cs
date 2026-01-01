// SPDX-FileCopyrightText: 2025 Shinagwa Kazemaru
// SPDX-License-Identifier: BSD-2-Clause license

using Unity.VisualScripting;
using UnityEngine;

public class Heat : MonoBehaviour {
    [SerializeField] float jumpImpulse;
    [SerializeField] float timeTilljump;

    Rigidbody[] segmentBodys;

    // Indexs of a collider s.t. each segment of the snake is touching the collider.
    int[] touchIndexs;
    // Time s.t. each segment of the snake is touching the collider.
    float[] touchTimes;


    public void OnSnakeTouch(int colliderIndex, Transform segment) {
        int segmentIndex = segment.GetSiblingIndex();
        if (segmentBodys[segmentIndex] is null || segmentBodys[segmentIndex].IsDestroyed())
            segmentBodys[segmentIndex] = segment.GetChild(0).GetComponent<Rigidbody>();
        touchIndexs[segmentIndex] = colliderIndex;
    }


    public void OnSnakeLeave(int colliderIndex, Transform segment) {
        int segmentIndex = segment.GetSiblingIndex();
        if (touchIndexs[segmentIndex] != colliderIndex)
            return;
        touchIndexs[segmentIndex] = -1;
        touchTimes[segmentIndex] = 0f;
    }


    void Start() {
        touchIndexs = new int[Snake.SegmentsCount];
        touchTimes = new float[Snake.SegmentsCount];
        segmentBodys = new Rigidbody[Snake.SegmentsCount];

        for (int i = 0; i < Snake.SegmentsCount; ++i) {
            touchIndexs[i] = -1;
            touchTimes[i] = 0f;
        }

        for (int j = transform.childCount - 1; j >= 0; --j)
            transform.GetChild(j).AddComponent<HotFloor>();
    }


    void Update() {}


    void FixedUpdate() {
        for (int i = 0; i < Snake.SegmentsCount; ++i) {
            // If the snake respawn when touching hot floor
            if (segmentBodys[i] is null || segmentBodys[i].IsDestroyed())
                touchIndexs[i] = -1;

            if (touchIndexs[i] < 0) continue;

            touchTimes[i] += Time.fixedDeltaTime;
            if (touchTimes[i] > timeTilljump) {
                touchTimes[i] = 0f;
                segmentBodys[i].AddForce(jumpImpulse * Vector3.up, ForceMode.Impulse);
            }
        }
    }
}
