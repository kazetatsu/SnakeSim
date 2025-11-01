using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    MusicArea[] areas;
    AudioSource source;
    int playingID = -1;

    void Start() {
        int n = transform.childCount;
        areas = new MusicArea[n];
        for (int i = 0; i < n; ++i)
            areas[i] = transform.GetChild(i).GetComponent<MusicArea>();

        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        int newPlayingID = -1;
        for (int i = 0; i < areas.Length; ++i)
        {
            if (areas[i].playing)
            {
                newPlayingID = i;
                break;
            }
        }

        if (playingID != newPlayingID)
        {
            playingID = newPlayingID;
            if (playingID < 0)
                source.Stop();
            else
            {
                source.clip = areas[playingID].clip;
                source.Play();
            }
        }

        if (playingID >= 0)
            source.volume = areas[playingID].volume;
    }
}
