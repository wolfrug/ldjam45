using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
    TRANSFORM_FORWARD = 1000,
    TRANSFORM_RIGHT = 2000,
    TRANSFORM_LEFT = 3000,
    TRANSFORM_BACK = 4000,
    TRANSFORM_DOWN = 5000,
    TRANSFORM_UP = 6000
}

public class DebrisShooter : MonoBehaviour {
    public GameObject[] debrisPrefabs;
    public List<EnemyDebris> debrisInStorage;
    [Tooltip ("Min/max frequency of shooting")]
    public Vector2 frequency = new Vector2 (1f, 2f);
    [Tooltip ("Min/max force applied to debris")]
    public Vector2 force = new Vector2 (50f, 150f);
    [Tooltip ("Min/max damage caused by hit")]
    public Vector2 damage = new Vector2 (5f, 5f);

    public Direction direction = Direction.TRANSFORM_FORWARD;

    // Start is called before the first frame update
    /*void Start () {
        StartCoroutine (Shooter ());
    } */

    IEnumerator Shooter () {
        while (enabled) {
            // wait until not paused
            yield return new WaitUntil (() => !GameManager.instance.paused);
            // wait for frequency
            yield return new WaitForSeconds (Random.Range (frequency.x, frequency.y));
            // spawn
            GameObject debris = Instantiate (debrisPrefabs[Random.Range (0, debrisPrefabs.Length)], transform);
            debris.transform.position = transform.position;
            debris.transform.SetParent (null);
            EnemyDebris component = debris.GetComponent<EnemyDebris> ();
            component.force = Random.Range (force.x, force.y);
            component.damage = Random.Range (damage.x, damage.y);
            component.direction = direction;
            if (component.allowPooling && !debrisInStorage.Contains (component)) {
                component.debrisHitEvent.AddListener (AddToStorage);
            };
        }
    }

    void AddToStorage (GameObject hit, EnemyDebris debris) {
        debris.debrisHitEvent.RemoveListener (AddToStorage);
        GameManager.instance.DelayedAction (2f, new System.Action (delegate { debris.ResetDebris (); debrisInStorage.Add (debris); }));
    }
    void AddDebris (EnemyDebris debris) {
        if (!debrisInStorage.Contains (debris)) {
            debrisInStorage.Add (debris);
        }
    }
    void OnEnable () {
        StartCoroutine (Shooter ());
    }

    // Update is called once per frame
    void Update () {

    }
}