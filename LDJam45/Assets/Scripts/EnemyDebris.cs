using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DebrisHit : UnityEvent<GameObject> { }

public class EnemyDebris : MonoBehaviour {
    public GameObject ps;
    public float force = 1000f;
    private Rigidbody rb;
    public DebrisHit debrisHitEvent;
    // Start is called before the first frame update
    void Start () {
        rb = GetComponent<Rigidbody> ();
        debrisHitEvent.AddListener (GameManager.instance.player.PlayerHit);
    }

    public void Hit (GameObject hitTarget) {
        Debug.Log ("Hit!");
        debrisHitEvent.Invoke (hitTarget);
        DestroyDebris ();
        //GameManager.instance.Restart ();
    }
    public void DestroyDebris () {
        ps.transform.parent = null;
        ps.SetActive (true);
        gameObject.SetActive (false);
    }

    void Update () {
        if (!GameManager.instance.paused) {
            rb.AddForce (transform.right * force);
        } else {
            rb.velocity = Vector3.zero;
        };
    }
}