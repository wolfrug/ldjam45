using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public static PlayerUI instance;

    public StatSlider redSlider;
    public StatSlider blueSlider;
    public StatSlider greenSlider;

    void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy (gameObject);
        }
    }

    // Start is called before the first frame update
    void Start () {
        GameManager.instance.statUpdateEvent.AddListener (UpdateSliders);
        UpdateSliders ();
    }

    public void UpdateSliders () {
        UpdateSliders (InteractableColor.NONE);
    }
    public void UpdateSliders (InteractableColor type) { // Listens to game manager events on updates
        redSlider.maxValue = GameManager.instance.RedStatMax;
        greenSlider.maxValue = GameManager.instance.GreenStatMax;
        blueSlider.maxValue = GameManager.instance.BlueStatMax;
        
    }

    // Update is called once per frame
    void Update () {

    }
}