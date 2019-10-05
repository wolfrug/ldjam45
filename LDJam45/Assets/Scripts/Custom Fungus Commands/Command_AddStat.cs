using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

[
    CommandInfo (
        "LUDUMDARE_JAM",
        "Stats/Add Stat",
        "Adds a number to one of the given stats. Note: capped between 0 and 100"
    )
]
[AddComponentMenu ("")]
public class Command_AddStat : Command {

    public InteractableColor stat;
    public FloatData amount;

    public override void OnEnter () {

        GameManager.instance.AddStat (stat, amount);
        Continue ();

    }
    public override string GetSummary () {
        if (stat == InteractableColor.NONE) {
            return "Select a color to add to";
        } else {
            return "";
        }
    }
    public override Color GetButtonColor () {
        return new Color32 (45, 198, 234, 255);
    }
}