using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public SimpleCharacterController controller;
    public GameObject shield;
    // Start is called before the first frame update
    void Start () {
        if (controller == null) {
            controller = GetComponent<SimpleCharacterController> ();
        }
        GameManager.instance.statUpdateEvent.AddListener (UpdatePlayerAbilities);
        shield.SetActive (false);
    }

    public void UpdatePlayerAbilities (InteractableColor color) {

        UpdatePlayerSpeedAndJump ();
        SetRegenSpeed (InteractableColor.GREEN, GameManager.instance.GreenStatMax / 5f);
        SetRegenSpeed (InteractableColor.RED, GameManager.instance.RedStatMax / 30f);
        SetRegenSpeed (InteractableColor.BLUE, GameManager.instance.BlueStatMax / 30f);
    }
    void UpdatePlayerSpeedAndJump () {
        SetPlayerSpeed (GameManager.instance.greenStat / 10f);
        SetPlayerJumpHeight (GameManager.instance.greenStat / 5f);
    }

    public void SetPlayerSpeed (float newSpeed) {
        controller.speed = Mathf.Clamp (newSpeed, 1f, 5f);
    }
    public void SetPlayerJumpHeight (float newHeight) {
        controller.jumpSpeed = Mathf.Clamp (newHeight, 0f, 20f);
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

    // Update is called once per frame
    void Update () {
        UpdatePlayerSpeedAndJump ();
        if (Input.GetAxis ("Vertical") > 0f && GameManager.instance.blueStat > 10f) {
            if (GameManager.instance.blueStat > 1f) {
                shield.SetActive (true);
                PlayerUI.instance.blueSlider.regenerating = false;
                GameManager.instance.blueStat = GameManager.instance.blueStat - 1f * Time.deltaTime;
            } else {
                PlayerUI.instance.blueSlider.regenerating = true;
                shield.SetActive (false);
            }
        } else {
            PlayerUI.instance.blueSlider.regenerating = true;
            shield.SetActive (false);
        }
    }
}