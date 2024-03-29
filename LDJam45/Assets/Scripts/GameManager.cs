﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StatsUpdated : UnityEvent<InteractableColor> { }

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public bool paused = false;
    private Player playerRef;

    [SerializeField]
    public float RedStatMax = 1f;
    [SerializeField]
    public float BlueStatMax = 1f;
    [SerializeField]
    public float GreenStatMax = 1f;
    [SerializeField]
    private float RedStatCur = 1f;
    [SerializeField]
    private float BlueStatCur = 0f;
    [SerializeField]
    private float GreenStatCur = 0f;
    public StatsUpdated statUpdateEvent;

    public List<InteractableData> finishedItems = new List<InteractableData> ();
    public List<SimpleEnemyAI> allEnemies = new List<SimpleEnemyAI> ();
    public Vector3 lastItem = Vector3.zero;
    public bool InBossScene = false;
    public CustomAudioSource audioSource;
    public AudioClip gainStatSound;
    private Coroutine playerKilledCR;

    void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy (this);
        }
    }
    // Start is called before the first frame update
    void Start () {
        if (MultiSceneLoader.instance != null) {
            MultiSceneLoader.instance.scenesLoadedEvent.AddListener (GameLoaded);
        } else {
            GameLoaded (new string[] { "", "" });
        }
        foreach (SimpleEnemyAI enemy in FindObjectsOfType<SimpleEnemyAI> ()) {
            allEnemies.Add (enemy);
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
    public float redStat {
        get {
            RedStatCur = PlayerUI.instance.redSlider.value;
            return RedStatCur;
        }
        set {
            RedStatCur = Mathf.Clamp (value, 0f, RedStatMax);
            PlayerUI.instance.redSlider.value = RedStatCur;
            statUpdateEvent.Invoke (InteractableColor.RED);
        }
    }
    public float greenStat {
        get {
            GreenStatCur = PlayerUI.instance.greenSlider.value;
            return GreenStatCur;
        }
        set {
            GreenStatCur = Mathf.Clamp (value, 0f, GreenStatMax);
            PlayerUI.instance.greenSlider.value = GreenStatCur;
            statUpdateEvent.Invoke (InteractableColor.GREEN);
        }
    }
    public float blueStat {
        get {
            BlueStatCur = PlayerUI.instance.blueSlider.value;
            return BlueStatCur;
        }
        set {
            BlueStatCur = Mathf.Clamp (value, 0f, BlueStatMax);
            PlayerUI.instance.blueSlider.value = BlueStatCur;
            statUpdateEvent.Invoke (InteractableColor.BLUE);
        }
    }

    public void AddStat (InteractableColor type, float amount) {
        switch (type) {

            case InteractableColor.BLUE:
                {
                    BlueStatMax += Mathf.Clamp (amount, 1f, 100f);
                    statUpdateEvent.Invoke (InteractableColor.BLUE);
                    break;
                }
            case InteractableColor.RED:
                {
                    RedStatMax += Mathf.Clamp (amount, 1f, 100f);
                    statUpdateEvent.Invoke (InteractableColor.RED);
                    break;
                }
            case InteractableColor.GREEN:
                {
                    GreenStatMax += Mathf.Clamp (amount, 1f, 100f);
                    statUpdateEvent.Invoke (InteractableColor.GREEN);
                    break;
                }
            default:
                {
                    Debug.LogWarning ("Did not assign any stat type to raise!");
                    break;
                }
        }
        PlayerUI.instance.UpdateSliders ();
        audioSource.SetClip(gainStatSound, false);
        audioSource.Play();
    }
    public float GetStatMax (InteractableColor type) {
        switch (type) {

            case InteractableColor.BLUE:
                {
                    return BlueStatMax;
                }
            case InteractableColor.RED:
                {
                    return RedStatMax;
                }
            case InteractableColor.GREEN:
                {
                    return GreenStatMax;
                }
            default:
                {
                    Debug.LogWarning ("Did not assign any stat type to raise!");
                    return 0f;
                }
        }
    }

    public void PauseGame (bool pause) {
        paused = pause;
        player.controller.EnableControl (!pause);
    }

    public void GameLoaded (string[] scenes) {
        if (scenes.Length > 0) {
            CameraManager.instance.Init ();
        };
    }
    public void DelayedAction (float delay, System.Action callBack) {
        StartCoroutine (ActionWaiter (delay, callBack));
    }
    IEnumerator ActionWaiter (float timeToWait, System.Action callBack) { // Usage: StartCoroutine (ActionWaiter (waitTime, new System.Action (() => RunTutorial (tutorialName))));
        yield return new WaitForSeconds (timeToWait);
        callBack.Invoke ();
    }

    public void PlayerKilled () { // only reload main scene
        if (playerKilledCR == null) {
            playerKilledCR = StartCoroutine (ReloadGamePlayScene ());
        };
    }
    IEnumerator ReloadGamePlayScene () {
        playerRef = null;
        if (!InBossScene) {
            yield return StartCoroutine (MultiSceneLoader.instance.ReloadOpenScene ("mainGameScene"));
            if (lastItem != Vector3.zero) {

                player.transform.position = new Vector3 (0f, 0f, lastItem.z);
            };
        } else {
            yield return StartCoroutine (MultiSceneLoader.instance.ReloadOpenScene ("bossFight"));
            Debug.Log ("Hmm");
        }
        PauseGame (false);
        CameraManager.instance.Init ();
        playerKilledCR = null;
    }
    IEnumerator LoadBossSceneAsync () {
        yield return MultiSceneLoader.instance.UnloadOpenScene ("mainGameScene");
        yield return MultiSceneLoader.instance.AddOpenScene ("bossFight");
        playerRef = null;
        CameraManager.instance.Init();
    }
    public void LoadBossScene () {
        InBossScene = true;
        StartCoroutine (LoadBossSceneAsync ());

    }
    public void LoadEndScene () {
        MultiSceneLoader.instance.LoadSceneExclusive ("endGame");
    }
    public void Restart () {
        MultiSceneLoader.instance.OpenSceneSet (MultiSceneLoader.instance.startingScenes);
    }

}