using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

[
    CommandInfo (
        "LUDUMDARE_JAM",
        "Stats/Get Stat",
        "Returns the current max value of a stat as a float"
    )
]
[AddComponentMenu ("")]
public class Command_GetStat : Command {

    public InteractableColor stat;

    [VariableProperty (typeof (FloatVariable))]
    public FloatVariable returnVariable;

    public override void OnEnter () {

        if (GameManager.instance != null && returnVariable != null) {
            returnVariable.Value = GameManager.instance.GetStatMax (stat);
        };
        Continue ();

    }
    public override string GetSummary () {
        if (stat == InteractableColor.NONE) {
            return "Select a color to add to";
        } else if (returnVariable == null) {
            return "You must assign a return variable!";
        } else return "Get Stat " + stat.ToString ();
    }
    public override Color GetButtonColor () {
        return new Color32 (45, 198, 234, 255);
    }
}