using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EnemyKilled : UnityEvent<SimpleEnemyAI> { }

public class SimpleEnemyAI : MonoBehaviour {

    public Transform[] wayPoints;
    public Transform currentLookTarget;
    public Animator animator;
    public float turn_speed = 1f;
    public float speed = 1f;

    public float readyDistance = 2f;
    public bool moving;
    public bool active = true;
    // Picks a random WP to begin from
    public bool randomizeStart = false;
    public CharacterController controller;
    public Vector2 minMaxWait = new Vector2 (0f, 1f);
    // Start is called before the first frame update
    private int currentWp = 0;
    private Vector3 moveDirection_ = Vector3.forward;
    private Coroutine animationCR;

    public EnemyKilled enemyKilledEvent;
    private bool forcedActive = false;

    public float health = 5f;
    void Start () {
        if (controller == null) {
            controller = GetComponent<CharacterController> ();
        }
    }

    void OnEnable () { // to handle activations/deactivations

        if (wayPoints.Length > 0) {
            active = true;
            moving = true;
            if (randomizeStart) {
                RandomizeStart ();
            }
            currentLookTarget = wayPoints[currentWp];
            Walk ();
        }
    }
    void RandomizeStart () {
        if (wayPoints.Length > 0) {
            currentWp = Random.Range (0, wayPoints.Length);
        }
    }

    public void ForceActive () { // For spoopy cutscenes
        forcedActive = true;
    }

    void WalkNext () {
        if (wayPoints.Length > 0) {
            if (wayPoints.Length > currentWp + 1) {
                currentWp++;
                currentLookTarget = wayPoints[currentWp];
            } else {
                {
                    currentWp = 0;
                    currentLookTarget = wayPoints[currentWp];
                }
            }
            if (animator != null) {
                animator.SetBool ("walking", false);
            }
            Invoke ("Walk", Random.Range (minMaxWait.x, minMaxWait.y));
        };
    }
    void Walk () {
        moving = true;
        if (animator != null) {
            animator.SetBool ("walking", true);
        }
    }

    Vector3 moveDirection {
        get {
            moveDirection_ = transform.forward * speed;
            return moveDirection_;
        }
        set {
            moveDirection_ = value;
        }
    }

    public void SpecialAttack (float waitTime) {

        if (animationCR == null) {
            animationCR = StartCoroutine (AnimatorWaiter (waitTime, "special", true));
        };
    }
    public void Attack (float waitTime) {
        if (animationCR == null) {
            animationCR = StartCoroutine (AnimatorWaiter (waitTime, "attack", true));
        };
    }
    public void Jump (float waitTime) {
        if (animationCR == null) {
            animationCR = StartCoroutine (AnimatorWaiter (waitTime, "jump", false));
        };
    }

    public void Hurt (float damage) {
        if (!animator.GetBool ("dead")) {
            health = health - damage * Time.deltaTime;
            animator.SetTrigger ("hit");
            if (health <= 0f) {
                Kill (0.1f);
            }
        };
    }
    public void GetHurtByStuff (GameObject target, EnemyDebris hitter) {
        if (target == gameObject) {
            Hurt (hitter.damage);
        }
    }

    public void Kill (float waitTime) {
        // We don't care about other animations, so kill them
        if (animationCR != null) {
            StopCoroutine (animationCR);
        }
        Activate (false);
        animationCR = StartCoroutine (AnimatorWaiter (waitTime, "die", true));
        animator.SetBool ("dead", true);
        enemyKilledEvent.Invoke (this);
    }

    public void HitPlayer (float damage) {
        if (!animator.GetBool ("dead") && !GameManager.instance.paused) {
            Player player = GameManager.instance.player;
            player.PlayerHit (damage * Time.deltaTime);
        };
    }

    IEnumerator AnimatorWaiter (float time, string animation, bool pauseMove) {
        bool activationStatus = active;

        yield return new WaitForSeconds (time);
        if (pauseMove) {
            Activate (false);
        }
        animator.SetTrigger (animation);
        yield return new WaitForSeconds (animator.GetCurrentAnimatorStateInfo (0).length + 1f);
        if (pauseMove) {
            Activate (activationStatus);
            moving = activationStatus;
        }
        animationCR = null;
    }

    public void Activate (bool activate) {
        active = activate;
        if (animator != null) {
            if (!activate) {
                moving = false;
                animator.SetBool ("walking", false);
            };
        }
    }

    void OnDrawGizmos () {
        if (wayPoints.Length > 0) {
            Gizmos.color = Color.red;
            Transform lastTransform = transform;
            for (int i = 0; i < wayPoints.Length; i++) {
                float radius = i == 0?.2f : i * .2f;
                if (wayPoints[i] == currentLookTarget) {
                    Gizmos.color = Color.blue;
                }
                Gizmos.DrawSphere (wayPoints[i].position, radius);
                Gizmos.color = Color.red;
                if (lastTransform != null) {
                    Gizmos.DrawLine (lastTransform.position, wayPoints[i].position);
                };
                lastTransform = wayPoints[i];
            };
        };
    }
    // Update is called once per frame
    void Update () {
        if (active && !GameManager.instance.paused || forcedActive) {
            if (currentLookTarget != null) {
                Quaternion _lookRotation =
                    Quaternion.LookRotation ((currentLookTarget.position - transform.position).normalized);
                transform.rotation =
                    Quaternion.Slerp (transform.rotation, _lookRotation, Time.deltaTime * turn_speed);
            }

            if (moving) {
                //controller.Move (moveDirection * Time.deltaTime);
                transform.position += moveDirection * Time.deltaTime;
                if (currentLookTarget != null) {
                    if (Vector3.Distance (transform.position, currentLookTarget.position) < readyDistance) {
                        moving = false;
                        WalkNext ();
                    }
                }
            }
        }
    }
}