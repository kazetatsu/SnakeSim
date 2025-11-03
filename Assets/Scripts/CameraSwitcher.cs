using Unity.Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour {
    int camerasCount;
    CinemachineCamera[] cameras;
    Vector3[] cameraPositions;
    int nearestCameraID = 0;
    int priorCameraID = 0;

    float switchDelay = .1f;
    float timeTillSwitch;


    void Start() {
        camerasCount = transform.childCount;
        cameras = new CinemachineCamera[camerasCount];
        cameraPositions = new Vector3[camerasCount];
        for (int i = 0; i < camerasCount; ++i) {
            Transform child = transform.GetChild(i);
            cameras[i] = child.GetComponent<CinemachineCamera>();
            cameraPositions[i] = child.position;
        }
        cameras[priorCameraID].Prioritize();
    }

    void Update() {
        int nearestNew = 0;
        float dMin = float.PositiveInfinity;
        for (int i = 0; i < camerasCount; ++i) {
            float d = (cameraPositions[i] - Snake.headPos).magnitude;
            if (d < dMin) {
                nearestNew = i;
                dMin = d;
            }
        }

        if (priorCameraID != nearestNew) {
            //    nearest camera of former frame
            // is nearest camera of current frame ?
            if (nearestCameraID == nearestNew)
                timeTillSwitch -= Time.deltaTime;
            else
                timeTillSwitch = switchDelay;
            nearestCameraID = nearestNew;
            if (timeTillSwitch <= 0f) {
                priorCameraID = nearestCameraID;
                cameras[priorCameraID].Prioritize();
            }
        }
    }
}
