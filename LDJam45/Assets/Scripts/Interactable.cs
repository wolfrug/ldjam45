using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

public class Interactable : MonoBehaviour {
    public InteractableData data;

    public void InteractWith (BasicAgent agent, GenericActivate source) {
        Debug.Log("Interacted with");
        Fungus.FungusManager.Instance.EventDispatcher.Raise (new AgentInteracted.Agent_EventHandlerEvent () { data = data });
    }
}