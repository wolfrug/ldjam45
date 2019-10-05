using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public static CameraManager instance;
    public Camera cam;
    public CinemachineVirtualCamera virtualCamera;
    void Awake () {
        if (instance == null) {
            instance = this;
            cam = GetComponent<Camera> ();
        } else {
            Destroy (this);
        }
    }
    // Start is called before the first frame update
    void Start () {

    }

    public void Init () {
        if (virtualCamera.LookAt == null) {
            virtualCamera.LookAt = GameManager.instance.player.transform;
        }
        if (virtualCamera.Follow == null) {
            virtualCamera.Follow = GameManager.instance.player.transform;
        }
    }

    // Update is called once per frame
    void Update () {

    }
}