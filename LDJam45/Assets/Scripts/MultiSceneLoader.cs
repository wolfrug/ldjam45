using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ScenesLoaded : UnityEvent<string[]> { }
public class MultiSceneLoader : MonoBehaviour { // Handles the loading of multiple scenes as necessary

    public static MultiSceneLoader instance;

    public bool runThis = true;

    [Tooltip ("The scenes you want to load directly on start")]
    public string[] startingScenes;

    [Tooltip ("The scenes that are currently loaded")]
    public List<string> currentlyLoadedScenes = new List<string> { };

    private Coroutine setOpener;

    public ScenesLoaded scenesLoadedEvent;

    // Start is called before the first frame update
    void Awake () { // We want this to be a singleton
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad (gameObject);
        } else {
            Destroy (this);
        }

    }
    void Start () {
        if (startingScenes.Length > 0 && instance == this && runThis) {
            OpenSceneSet (startingScenes);
        }
    }
    public void OpenStartingScenes () {
        if (startingScenes.Length > 0 && instance == this) {
            OpenSceneSet (startingScenes);
        }
    }
    public void OpenSceneSet (string[] scenesToOpen) { // opens this set of scenes and closes all others
        if (scenesToOpen.Length > 1) {
            if (setOpener == null) {
                setOpener = StartCoroutine (LoadScenesInOrder (scenesToOpen));
            }
        } else if (scenesToOpen.Length == 1) {
            LoadSceneExclusive (scenesToOpen[0]);
        } else {
            Debug.LogWarning ("Cannot complete OpenSceneSet: no scenes given!");
        }
    }
    public AsyncOperation LoadSceneExclusive (string sceneToLoad) { // loads ONLY this scene
        AsyncOperation syncOp = SceneManager.LoadSceneAsync (sceneToLoad, LoadSceneMode.Single);
        currentlyLoadedScenes.Clear ();
        currentlyLoadedScenes.Add (sceneToLoad);
        return syncOp;
    }
    IEnumerator LoadScenesInOrder (string[] scenes) {
        if (scenes.Length > 1) {
            // Load scene 0 exlusively
            AsyncOperation syncOp = LoadSceneExclusive (scenes[0]);
            yield return new WaitUntil (() => syncOp.isDone);
            // Load the rest of the scenes in order
            for (int i = 1; i < scenes.Length; i++) {
                syncOp = AddOpenScene (scenes[i]);
                if (syncOp != null) {
                    yield return new WaitUntil (() => syncOp.isDone);
                };
            }
        } else {
            Debug.LogWarning ("Cannot load scenes in order as fewer than 2 scenes were provided");
        }
        // Run the event
        scenesLoadedEvent.Invoke (scenes);
        setOpener = null;
    }
    public AsyncOperation AddOpenScene (string sceneToAdd) { // Returns an asyncoperation, which can be checked for .isDone to see if the scene is finished loading
        AsyncOperation syncOp = null;
        if (!currentlyLoadedScenes.Contains (sceneToAdd)) {
            syncOp = SceneManager.LoadSceneAsync (sceneToAdd, LoadSceneMode.Additive);
            currentlyLoadedScenes.Add (sceneToAdd);
        };
        return syncOp;
    }
    public AsyncOperation UnloadOpenScene (string sceneToUnload) {
        AsyncOperation syncOp = null;
        if (currentlyLoadedScenes.Contains (sceneToUnload)) {
            syncOp = SceneManager.UnloadSceneAsync (sceneToUnload);
            currentlyLoadedScenes.Remove (sceneToUnload);
        }
        return syncOp;
    }
}