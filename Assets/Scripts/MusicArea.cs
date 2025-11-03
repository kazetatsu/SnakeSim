using UnityEngine;

public class MusicArea : MonoBehaviour
{
    static float disableHeight = .5f;
    static float fadeHeight = 3f;

    [SerializeField] float height;
    public bool playing;
    public float volume = 1f;
    public AudioClip clip;


    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(32f, 2f*height, 32f));
    }


    void Update() {
        float y = Snake.headPos.y - transform.position.y;
        volume = Mathf.Min(1f, (height - y) / fadeHeight, (height + y) / fadeHeight);
        volume = Mathf.Max(0f, volume);

        if (volume > 0f) // volume > 0  <=>  -height < y < height
            playing = true;
        else if (y < -height - disableHeight || height + disableHeight < y)
            playing = false;
    }
}
