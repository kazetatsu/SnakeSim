using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] float delay;


    void OnTriggerEnter(Collider _) {
        GameObject.Find("SceneLoader")?.GetComponent<SceneLoader>().LoadWithCurtain(delay, "Ending");
        Destroy(this);
    }
}
