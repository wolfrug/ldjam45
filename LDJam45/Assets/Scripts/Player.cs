﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public SimpleCharacterController controller;
    public SpriteRenderer sprite;
    public GameObject shield;
    public GameObject attack;
    public GameObject levitate;
    public Animator floatAnimator;
    public Animator playerAnimator;
    public bool shieldOn = false;
    public bool attackCharging = false;
    public bool floatOn = false;
    public bool shieldCooldown = false;
    public bool attackCooldDown = false;
    public bool floatCoolDown = false;

    public CustomAudioSource audioSource_fly;
    public CustomAudioSource audioSource_attack;
    public CustomAudioSource audioSource_shield;
    public CustomAudioSource audioSource_self;

    // Start is called before the first frame update
    void Start () {
        if (controller == null) {
            controller = GetComponent<SimpleCharacterController> ();
        }
        GameManager.instance.statUpdateEvent.AddListener (UpdatePlayerAbilities);
        shield.SetActive (false);
        attack.SetActive (false);
        levitate.SetActive (false);
        UpdatePlayerAbilities (InteractableColor.NONE);
        audioSource_attack.PausePlay(true);
        audioSource_fly.PausePlay(true);
        audioSource_shield.PausePlay(true);
    }

    public void UpdatePlayerAbilities (InteractableColor color) {

        UpdatePlayerSpeedAndJump ();
        SetRegenSpeed (InteractableColor.GREEN, GameManager.instance.GreenStatMax / 5f);
        SetRegenSpeed (InteractableColor.RED, GameManager.instance.RedStatMax / 5f);
        SetRegenSpeed (InteractableColor.BLUE, GameManager.instance.BlueStatMax / 5f);
    }
    void UpdatePlayerSpeedAndJump () {
        SetPlayerSpeed (GameManager.instance.GreenStatMax / 10f);
        SetPlayerJumpHeight (GameManager.instance.GreenStatMax / 5f);
    }

    public void SetPlayerSpeed (float newSpeed) {
        controller.speed = Mathf.Clamp (newSpeed, 3f, 5f);
    }
    public void SetPlayerJumpHeight (float newHeight) {
        controller.jumpSpeed = Mathf.Clamp (newHeight, 8f, 20f);
    }
    public void SetRegenSpeed (InteractableColor type, float newRegen) {
        switch (type) {
            case InteractableColor.RED:
                {
                    PlayerUI.instance.redSlider.regenSpeed = newRegen;
                    break;
                }
            case InteractableColor.BLUE:
                {
                    PlayerUI.instance.blueSlider.regenSpeed = newRegen;
                    break;
                }
            case InteractableColor.GREEN:
                {
                    PlayerUI.instance.greenSlider.regenSpeed = newRegen;
                    break;
                }
            default:
                {
                    Debug.LogWarning ("Tried to set regen speed on null value!");
                    break;
                }
        }
    }

    public void PlayerHit (float damage) {
        if (!shieldOn) {
            playerAnimator.SetTrigger ("Hit");
            GameManager.instance.redStat = GameManager.instance.redStat - damage;
            if (GameManager.instance.redStat <= 0f) {
                KillPlayer ();
            }
        }
    }
    public void PlayerHit (GameObject hit, EnemyDebris hitter) {
        if (hit == gameObject) {
            audioSource_self.Play();
            PlayerHit (hitter.damage);
        };
    }
    public void KillPlayer () {
        floatAnimator.SetTrigger ("Kill");
        playerAnimator.SetTrigger("Die");
        GameManager.instance.PauseGame (true);
        GameManager.instance.DelayedAction (3f, new System.Action (() => GameManager.instance.PlayerKilled ()));
        enabled = false;
    }

    // Update is called once per frame
    void Update () {
        //UpdatePlayerSpeedAndJump ();
        if (Input.GetAxis ("Vertical") > 0f) { // arrow up - shield
            if (GameManager.instance.blueStat > 1f && !shieldCooldown) {
                shieldOn = true;
                shield.SetActive (true);
                PlayerUI.instance.blueSlider.regenerating = false;
                GameManager.instance.blueStat = GameManager.instance.blueStat - 5f * Time.deltaTime;
                audioSource_shield.PausePlay(false);
            } else if (shieldCooldown && GameManager.instance.blueStat > 10f) {
                shieldCooldown = false;
            } else {
                PlayerUI.instance.blueSlider.regenerating = true;
                shield.SetActive (false);
                shieldOn = false;
                shieldCooldown = true;
                audioSource_shield.PausePlay(true);
            }
        } else if (Input.GetAxis ("Vertical") < 0f) { // arrow down - attack
            if (GameManager.instance.redStat > 1f && !attackCooldDown) {
                attackCharging = true;
                attack.SetActive (true);
                PlayerUI.instance.redSlider.regenerating = false;
                GameManager.instance.redStat = GameManager.instance.redStat - 5f * Time.deltaTime;
                audioSource_attack.PausePlay(false);
            } else if (attackCooldDown && GameManager.instance.redStat > 10f) {
                attackCooldDown = false;
            } else {
                PlayerUI.instance.redSlider.regenerating = true;
                attack.SetActive (false);
                attackCharging = false;
                attackCooldDown = true;
                audioSource_attack.PausePlay(true);
            }
        } else {
            if (shieldOn) {
                PlayerUI.instance.blueSlider.regenerating = true;
                shield.SetActive (false);
                shieldOn = false;
                audioSource_shield.PausePlay(true);
            };
            if (attackCharging) {
                PlayerUI.instance.redSlider.regenerating = true;
                attack.SetActive (false);
                attackCharging = false;
                audioSource_attack.PausePlay(true);
            }
        }
        if (Input.GetAxis ("Jump") > 0f) { // float time!
            if (GameManager.instance.greenStat > 1f && !floatCoolDown) {
                floatOn = true;
                levitate.SetActive (true);
                controller.gravity = -1f * Time.deltaTime;
                PlayerUI.instance.greenSlider.regenerating = false;
                GameManager.instance.greenStat = GameManager.instance.greenStat - 5f * Time.deltaTime;
                audioSource_fly.PausePlay(false);
            } else if (floatCoolDown && GameManager.instance.greenStat > 10f) {
                floatCoolDown = false;
            } else {
                PlayerUI.instance.greenSlider.regenerating = true;
                controller.gravity = controller.defaultGravity;
                floatOn = false;
                levitate.SetActive (false);
                floatCoolDown = true;
                audioSource_fly.PausePlay(true);
            }
        } else if (floatOn) {
            controller.gravity = controller.defaultGravity;
            floatOn = false;
            levitate.SetActive (false);
            PlayerUI.instance.greenSlider.regenerating = true;
            audioSource_fly.PausePlay(true);
        }
        if (Input.GetAxis ("Horizontal") > 0f && !attackCharging) {
            sprite.flipX = true;
        } else {
            sprite.flipX = false;
        }
        if (shieldOn || attackCharging || floatOn) {

        }
        //Debug.Log("Transform position y:" + transform.position.y);
        // Zone of death
        if (transform.position.y < -50f) {
            KillPlayer ();
        }
        if (transform.position.x > 0f || transform.position.x < 0f) { // To prevent moving in X
            transform.position = new Vector3 (0f, transform.position.y, transform.position.z);
        }
    }

}