using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public static CameraManager instance;
    public Camera cam;
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

    // Update is called once per frame
    void Update () {

    }
}