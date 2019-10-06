using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatSlider : MonoBehaviour {
    public InteractableColor color;
    public Image currentMax;
    public Image currentValue;
    public float baseSize = 10;
    public float regenSpeed = 1f;
    public bool regenerating = true;
    // Start is called before the first frame update
    void Start () {
        GameManager.instance.statUpdateEvent.AddListener (UpdateSelf);
        UpdateSelf (color);
    }

    public float maxValue {
        get {
            return currentMax.fillAmount * 100f;
        }
        set {
            currentMax.fillAmount = value / 100f;
        }
    }
    public float value {
        get {
            return currentValue.fillAmount * 100f;
        }
        set {
            currentValue.fillAmount = value / 100f;
        }
    }
    void UpdateSelf (InteractableColor updateColor) {
        //Debug.Log ("Received update for " + updateColor + " for slider " + color);
        if (color == updateColor) {
            maxValue = GameManager.instance.GetStatMax (color);
            Mathf.Clamp (value, 0f, maxValue);
        }
    }

    // Update is called once per frame
    void Update () {
        if (regenerating) {
            if (currentValue.fillAmount < currentMax.fillAmount) {
                Mathf.Clamp (value += Time.deltaTime * regenSpeed, 0f, maxValue);
            }
        }
    }
}