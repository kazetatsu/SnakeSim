using UnityEngine;
using UnityEngine.UI;

public class Curtain : MonoBehaviour
{
    [SerializeField] int separate;

    [Range(0f,1f)] public float open;

    GameObject[] parts;
    int pivot;


    void Start() {
        var rect = GetComponent<RectTransform>().rect;

        var partsPlaced = new bool[separate,separate];
        for (int r = 0; r < separate; ++r)
            for (int c = 0; c < separate; ++c)
                partsPlaced[r,c] = false;

        int n = separate * separate;
        int x = separate / 2;
        int y = separate / 2;
        int dx = 0;
        int dy = -1;
        float w = 1.01f * rect.width  / (float)separate;
        float h = 1.01f * rect.height / (float)separate;

        parts = new GameObject[n];
        parts[0] = transform.Find("Part 00").gameObject;
        var rt = parts[0].GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(w, h);
        rt.anchoredPosition = new Vector2((float)x * w, (float)y * h);
        partsPlaced[x, y] = true;

        for (int i = 1; i < n; ++i) {
            x += dx;
            y += dy;
            parts[i] = Instantiate(parts[0], transform);
            parts[i].name = "Part " + i.ToString("D2");
            parts[i].GetComponent<RectTransform>().anchoredPosition = new Vector2((float)x * w, (float)y * h);
            partsPlaced[x, y] = true;

            // No part exists on right => Turn right
            if (!partsPlaced[x+dy, y-dx]) {
                int temp = dx;
                dx = dy;
                dy = -temp;
            }
        }

        pivot = 0;
    }


    void Update() {
        int n = parts.Length;
        int ti = Mathf.CeilToInt((float)n * open);
        if (ti < 0) ti = 0;
        if (ti > n) ti = n;

        while (pivot < ti) {
            parts[pivot].SetActive(false);
            ++pivot;
        }
        while (pivot > ti) {
            --pivot;
            parts[pivot].SetActive(true);
        }
    }
}
