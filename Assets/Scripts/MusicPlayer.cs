using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    int areasLength;
    MusicArea[] areas;
    AudioSource source;
    int playingID = -1; // negative := snake isn't in any muisc areas 


    void Start() {
        areasLength = transform.childCount;
        areas = new MusicArea[areasLength];
        for (int i = 0; i < areasLength; ++i)
            areas[i] = transform.GetChild(i).GetComponent<MusicArea>();

        source = GetComponent<AudioSource>();
    }


    void Update() {
        int newPlayingID = -1;
        for (int i = 0; i < areasLength; ++i) {
            if (areas[i].playing) {
                newPlayingID = i;
                break;
            }
        }

        if (playingID != newPlayingID) {
            playingID = newPlayingID;
            if (playingID < 0)
                source.Stop();
            else {
                source.clip = areas[playingID].clip;
                source.Play();
            }
        }

        if (playingID >= 0)
            source.volume = areas[playingID].volume;
    }
}
