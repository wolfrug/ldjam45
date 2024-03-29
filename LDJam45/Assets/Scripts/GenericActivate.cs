﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class Activated : UnityEvent<BasicAgent, GenericActivate> { }

[System.Serializable]
public class ActivateStarted : UnityEvent<BasicAgent, GenericActivate> { };

[System.Serializable]
public class ActivateCancelled : UnityEvent<BasicAgent, GenericActivate> { };
public class GenericActivate : MonoBehaviour {
    public Canvas promptCanvas;
    public GameObject interactableTarget;
    public Animator animator;
    public float holdDownTimeSeconds = 0f;
    public float holdDownTimeCurrent = -1f;
    public string inputButton = "Fire1";
    public Activated activateEvent;
    public ActivateStarted activateStartedEvent;
    public ActivateCancelled activateCancelledEvent;
    [SerializeField]
    private bool awaitingInput = false;

    [Tooltip ("Disable after this many activations - use -1 to never disable")]
    public int disableAfterActivations = -1;
    public BasicAgent agent;

    void Start () {
        MakeVisible (false, null);
        if (animator == null) {
            animator = GetComponent<Animator> ();
        }
    }

    public void TriggerActivate (GameObject dynamic) {
        if (enabled) {
            BasicAgent tryAgent;
            tryAgent = dynamic.GetComponentInChildren<BasicAgent> ();
            if (tryAgent != null) {
                MakeVisible (true, tryAgent);
            }
        };
    }
    public void TriggerDeactivate (GameObject dynamic) {
        if (enabled) {
            BasicAgent tryAgent;
            tryAgent = dynamic.GetComponentInChildren<BasicAgent> ();
            if (tryAgent != null) {
                if (tryAgent == agent) {
                    MakeVisible (false, tryAgent);
                };
            }
        };
    }

    public void MakeVisible (bool visible, BasicAgent activatingAgent) {
        if (enabled) {
            promptCanvas.gameObject.SetActive (visible);
            awaitingInput = visible;
            agent = activatingAgent;
        };
    }
    public void MakeInvisible () {
        promptCanvas.gameObject.SetActive (false);
        awaitingInput = false;
        agent = null;
    }

    public void Activate () {
        if (disableAfterActivations > 0 || disableAfterActivations < 0) {
            activateEvent.Invoke (agent, this);
            disableAfterActivations--;
            if (disableAfterActivations == 0) {
                MakeInvisible ();
                enabled = false;
            };
        }
    }

    public void AddActivations (int amount) { // use this to either stop a permanent activation thing, or to re-activate a de-activated thing
        disableAfterActivations += amount;
        if (disableAfterActivations == 0) {
            MakeInvisible ();
            enabled = false;
        } else {
            enabled = true;
        }
    }

    void Update () {
        if (awaitingInput && enabled) {
            if (Input.GetAxis (inputButton) > 0f && holdDownTimeCurrent < 0f) {
                activateStartedEvent.Invoke (agent, this);
                holdDownTimeCurrent = 0f;
                if (animator != null) {
                    animator.SetTrigger ("fill");
                }
            } else if (Input.GetAxis (inputButton) > 0f && holdDownTimeCurrent >= 0f) {
                holdDownTimeCurrent += Time.deltaTime;
                animator.SetFloat ("speed", 1f / holdDownTimeSeconds);
                if (holdDownTimeCurrent >= holdDownTimeSeconds) {
                    Activate ();
                    holdDownTimeCurrent = -1f;
                }
            } else if (holdDownTimeCurrent > 0f) {
                holdDownTimeCurrent -= Time.deltaTime;
                animator.SetFloat ("speed", -1f / holdDownTimeSeconds);
            }
        } else {
            if (holdDownTimeCurrent > 0f) {
                holdDownTimeCurrent = -1f;
                activateCancelledEvent.Invoke (agent, this);
                animator.SetTrigger ("empty");
            }
        }
    }
}