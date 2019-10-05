using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    private Player playerRef;

    void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy (this);
        }
    }

    public Player player {
        get {
            if (playerRef == null) {
                playerRef = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
            }
            return playerRef;
        }
    }

    // Start is called before the first frame update
    void Start () {
        if (MultiSceneLoader.instance != null) {
            MultiSceneLoader.instance.scenesLoadedEvent.AddListener (GameLoaded);
        }
    }

    public void GameLoaded (string[] scenes) {
        if (scenes.Length > 0) {
            CameraManager.instance.Init ();
        };
    }

}