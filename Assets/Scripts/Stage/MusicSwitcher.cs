using UnityEngine;

public class MusicSwitcher : MonoBehaviour
{
    const float FadeTime = 0.7f;
    int areasLength;
    SubstageArea[] areas;
    AudioSource source;
    bool isSwitching = false;
    float dvoldt; // d volume / dt
    AudioClip nextClip;


    public void Switch(AudioClip clip) {
        nextClip = clip;
        dvoldt = -1f / FadeTime;
        isSwitching = true;
    }


    void Start() {
        areasLength = transform.childCount;
        areas = new SubstageArea[areasLength];
        for (int i = 0; i < areasLength; ++i)
            areas[i] = transform.GetChild(i).GetComponent<SubstageArea>();

        source = GetComponent<AudioSource>();
    }


    void Update() {
        if (!isSwitching) return;

        source.volume += dvoldt * Time.deltaTime;

        if (dvoldt > 0f && source.volume >= 1f)
            isSwitching = false;

        if (dvoldt < 0f && source.volume <= 0f) {
            if (nextClip is null) {
                source.Stop();
                isSwitching = false;
            } else {
                source.clip = nextClip;
                source.Play();
                dvoldt = 1f / FadeTime;
            }
        }
    }
}
