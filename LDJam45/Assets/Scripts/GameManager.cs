using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StatsUpdated : UnityEvent<InteractableColor> { }

public class GameManager : MonoBehaviour {
    public static GameManager instance;

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

    public void Restart(){
        MultiSceneLoader.instance.OpenSceneSet(MultiSceneLoader.instance.startingScenes);
    }

}