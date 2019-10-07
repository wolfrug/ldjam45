using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TriggerEntered : UnityEvent<GameObject> { }

[System.Serializable]
public class TriggerExited : UnityEvent<GameObject> { }

[System.Serializable]
public class TriggerStay : UnityEvent<GameObject> { };
public class GenericTrigger : MonoBehaviour {

    public List<string> tags = new List<string> { };
    public TriggerEntered triggerEntered;
    public TriggerExited triggerExited;
    public TriggerStay triggerStay;
    public bool useStay = true;
    public List<GameObject> contents = new List<GameObject> { };
    private List<GameObject> objectsFlaggedForRemoval = new List<GameObject> ();

    // Start is called before the first frame update
    void Start () { }

    void OnTriggerEnter (Collider other) {
        if (enabled) {
            if (tags.Contains (other.transform.tag) || tags.Count == 0) {
                if (!contents.Contains (other.gameObject)) {
                    triggerEntered.Invoke (other.gameObject);
                    contents.Add (other.gameObject);
                    //Debug.Log(other.gameObject);
                };
            }
        };
    }
    void OnTriggerExit (Collider other) {
        if (enabled) {
            if (contents.Contains (other.gameObject)) {
                contents.Remove (other.gameObject);
                triggerExited.Invoke (other.gameObject);
            }
        };
    }

    void FixedUpdate () {

        if (useStay) {
            if (contents.Count > 0) {
                objectsFlaggedForRemoval.Clear ();
                for (int i = 0; i < contents.Count; i++) {
                    {
                        GameObject obj = contents[i];
                        if (obj.activeInHierarchy) {
                            triggerStay.Invoke (obj);
                        } else {
                            objectsFlaggedForRemoval.Add (obj);
                        }
                    }
                };
                foreach (GameObject obj in objectsFlaggedForRemoval) {
                    contents.Remove (obj);
                }
            };
        }
    }
}