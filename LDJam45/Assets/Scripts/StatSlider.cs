using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatSlider : MonoBehaviour {
    public InteractableColor color;
    public Slider slider;
    public float baseSize = 10;
    public float regenSpeed = 1f;
    public bool regenerating = true;
    private RectTransform rectTransform;
    private float deltaHeight;
    // Start is called before the first frame update
    void Start () {
        if (slider == null) {
            slider = GetComponent<Slider> ();
        }
        rectTransform = GetComponent<RectTransform> ();
        deltaHeight = rectTransform.sizeDelta.y;
        GameManager.instance.statUpdateEvent.AddListener (UpdateSelf);
        UpdateSelf (color);
    }

    public float maxValue {
        get {
            return slider.maxValue;
        }
        set {
            slider.maxValue = value;
        }
    }
    public float value {
        get {
            return slider.value;
        }
        set {
            slider.value = value;
        }
    }
    void UpdateSelf (InteractableColor updateColor) {
        Debug.Log ("Received update for " + updateColor + " for slider " + color);
        if (color == updateColor) {
            rectTransform.sizeDelta = new Vector2 (baseSize + GameManager.instance.GetStatMax (color), deltaHeight);
        }
    }

    // Update is called once per frame
    void Update () {
        if (regenerating) {
            slider.value += Time.deltaTime * regenSpeed;
        }
    }
}