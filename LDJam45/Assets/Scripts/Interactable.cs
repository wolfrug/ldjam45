using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour {
    public InteractableData data;
    public Image itemImage; 
    public Text itemText;
    public MeshRenderer meshRenderer;
    // RED, GREEN, BLUE
    public Material[] materials;
    public Light lightSource;

    public void InteractWith (BasicAgent agent, GenericActivate source) {
        Debug.Log ("Interacted with " + gameObject);
        FungusManager.Instance.EventDispatcher.Raise (new AgentInteracted.Custom_EventHandlerEvent () { data = data });
    }

    void Start () { // set the color of stuff based on its type
    /*
        if (meshRenderer != null && lightSource != null && data != null && materials.Length == 3) {
            switch (data.type) {
                case InteractableColor.RED:
                    {
                        meshRenderer.material = materials[0];
                        lightSource.color = Color.red;
                        break;
                    }
                case InteractableColor.GREEN:
                    {
                        meshRenderer.material = materials[1];
                        lightSource.color = Color.green;
                        break;
                    }
                case InteractableColor.BLUE:
                    {
                        meshRenderer.material = materials[2];
                        lightSource.color = Color.blue;
                        break;
                    }
            }
        } */
        itemImage.sprite = data.image;
        itemText.text = data.name_;

    }
}