// SPDX-FileCopyrightText: 2025 Shinagwa Kazemaru
// SPDX-License-Identifier: BSD-2-Clause license

using UnityEngine;

public class Sign : MonoBehaviour
{
    [SerializeField] Texture2D tex;


    void Start() {
        transform.Find("Paper")
            .GetComponent<MeshRenderer>()
            .material
            .SetTexture("_BaseMap", tex);
    }
}
