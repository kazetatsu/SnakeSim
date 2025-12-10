using UnityEngine;
using UnityEngine.UI;

public class SoundVolume : MonoBehaviour
{
    [SerializeField] AudioScaler audioScaler;
    Slider slider;


    void Start() {
        slider = GetComponent<Slider>();
        slider.value = audioScaler.DefaultVolume;
    }


    void Update() {
        audioScaler.DefaultVolume = slider.value;
    }
}
