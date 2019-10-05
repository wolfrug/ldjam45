using System.Collections;
using Fungus;
using UnityEngine;

[EventHandlerInfo ("AGENT",
    "Agent Interacted With",
    "Runs when an agent (e.g. the player) has finished interacting with an interactable.")]
[AddComponentMenu ("")]
public class AgentInteracted : Custom_Fungus_EventHandler {

    /*
    Raising the event:
    Fungus.FungusManager.Instance.EventDispatcher.Raise(new Custom_Fungus_EventHandler.Custom_EventHandlerEvent() { text = "whatever" });

     */

    public InteractableData target;

    public class Agent_EventHandlerEvent {
        public InteractableData data;
    }

    protected override void OnEnable () {
        eventDispatcher = FungusManager.Instance.EventDispatcher;

        eventDispatcher.AddListener<Agent_EventHandlerEvent> (OnCustom_EventHandlerEvent);
    }

    protected override void OnDisable () {
        eventDispatcher.RemoveListener<Agent_EventHandlerEvent> (OnCustom_EventHandlerEvent);

        eventDispatcher = null;
    }

    void OnCustom_EventHandlerEvent (Agent_EventHandlerEvent evt) {

        Debug.Log ("Event raised: " + evt.data);
        if (evt.data == target) {
            ExecuteBlock ();
            return;
        };
    }
}