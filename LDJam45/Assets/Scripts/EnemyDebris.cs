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

    public Direction direction = Direction.TRANSFORM_FORWARD;
    private Vector3 actualDirection;
    private Rigidbody rb;
    public DebrisHit debrisHitEvent;
    public bool alsoHitEnemies = false;
    // Start is called before the first frame update
    void Start () {
        rb = GetComponent<Rigidbody> ();
        debrisHitEvent.AddListener (GameManager.instance.player.PlayerHit);
        if (alsoHitEnemies) {
            foreach (SimpleEnemyAI enemy in GameManager.instance.allEnemies) {
                debrisHitEvent.AddListener (enemy.GetHurtByStuff);
            }
        }
    }

    public void ResetDebris () {
        ps.SetActive (false);
        ps.transform.SetParent (transform);
        ps.transform.position = transform.position;
        gameObject.SetActive (true);
    }
    public void Hit (GameObject hitTarget) {
       // Debug.Log ("Hit!");
        debrisHitEvent.Invoke (hitTarget, this);
        DestroyDebris ();
        //GameManager.instance.Restart ();
    }
    public void DestroyDebris () {

        ps.transform.parent = null;
        ps.SetActive (true);
        gameObject.SetActive (false);

    }

    Vector3 ConvertDirection (Direction direction) {
        switch (direction) {
            case Direction.TRANSFORM_FORWARD:
                {
                    return transform.forward;
                }
            case Direction.TRANSFORM_BACK:
                {
                    return -transform.forward;
                }
            case Direction.TRANSFORM_LEFT:
                {
                    return -transform.right;
                }
            case Direction.TRANSFORM_RIGHT:
                {
                    return transform.right;
                }
            case Direction.TRANSFORM_DOWN:
                {
                    return -transform.up;
                }
            case Direction.TRANSFORM_UP:
                {
                    return transform.up;
                }
            default:
                {
                    return Vector3.zero;
                }
        }
    }

    void OnCollisionEnter (Collision other) {
       // Debug.Log ("Collided!");
        DestroyDebris ();
    }

    void Update () {
        if (!GameManager.instance.paused) {
            rb.AddForce (ConvertDirection (direction) * force);
        } else {
            rb.velocity = Vector3.zero;
        };
    }
}