using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

[
    CommandInfo (
        "LUDUMDARE_JAM",
        "Pause Game",
        "Pauses or unpauses the game (e.g. during narrative)"
    )
]
[AddComponentMenu ("")]
public class Command_PauseGame : Command {

    public BooleanData pause = new BooleanData (true);

    public override void OnEnter () {

        if (GameManager.instance != null) {
            GameManager.instance.PauseGame (pause);
        };
        Continue ();

    }
    public override string GetSummary () {
        if (pause) {
            return "Pausing game";
        } else {
            return "Unpausing game";
        }
    }
    public override Color GetButtonColor () {
        return new Color32 (45, 198, 234, 255);
    }
}