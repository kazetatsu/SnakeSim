using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioScaler : MonoBehaviour
{
    [SerializeField] string audioName;
    AudioSource _audio;
    float defaultVolume;
    public float DefaultVolume {
        get => defaultVolume;
        set {defaultVolume = Mathf.Clamp01(value);}
    }
    float tempVolume = 1f;
    public float TempVolume {
        get => tempVolume;
        set {tempVolume = Mathf.Clamp01(value);}
    }


    void SaveDefaultVolume(Scene _) {
        PlayerPrefs.SetFloat(audioName + "Volume", defaultVolume);
    }


    void Start() {
        _audio = GetComponent<AudioSource>();
        defaultVolume = PlayerPrefs.GetFloat(audioName + "Volume", 0.5f);
        SceneManager.sceneUnloaded += SaveDefaultVolume;
    }


    void Update() {
        float dstVolume = defaultVolume * tempVolume;
        if (dstVolume < 0f) dstVolume = 0f;
        if (dstVolume > 1f) dstVolume = 1f;

        if (_audio.volume != dstVolume)
            _audio.volume = dstVolume;
    }
}
