using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DebrisHit : UnityEvent<GameObject, EnemyDebris> { }

public class EnemyDebris : MonoBehaviour {
    public GameObject ps;
    public float force = 1000f;
    public float damage = 5f;
    public bool allowPooling = true;

    public Vector3 direction = Vector3.forward;
    private Rigidbody rb;
    public DebrisHit debrisHitEvent;
    // Start is called before the first frame update
    void Start () {
        rb = GetComponent<Rigidbody> ();
        debrisHitEvent.AddListener (GameManager.instance.player.PlayerHit);
    }

    public void ResetDebris () {
        ps.SetActive (false);
        ps.transform.SetParent (transform);
        ps.transform.position = transform.position;
        gameObject.SetActive (true);
    }
    public void Hit (GameObject hitTarget) {
        Debug.Log ("Hit!");
        debrisHitEvent.Invoke (hitTarget, this);
        DestroyDebris ();
        //GameManager.instance.Restart ();
    }
    public void DestroyDebris () {
        ps.transform.parent = null;
        ps.SetActive (true);
        gameObject.SetActive (false);
    }

    void OnCollisionEnter (Collision other) {
        Debug.Log ("Collided!");
        DestroyDebris ();
    }

    void Update () {
        if (!GameManager.instance.paused) {
            rb.AddForce (direction * force);
        } else {
            rb.velocity = Vector3.zero;
        };
    }
}