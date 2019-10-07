using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

public enum finishType {
    PRE_BOSS_FINISH = 0000,
    POST_BOSS_FINISH = 1000
}

[
    CommandInfo (
        "LUDUMDARE_JAM",
        "Finish Game",
        "Use this to tell the game it's time to move on to the next section/finish completely"
    )
]
[AddComponentMenu ("")]
public class Command_FinishGame : Command {

    public finishType typeOfFinish = finishType.PRE_BOSS_FINISH;

    public override void OnEnter () {

        if (typeOfFinish == finishType.PRE_BOSS_FINISH) {
            GameManager.instance.LoadBossScene ();
        } else {
            GameManager.instance.LoadEndScene ();
        }

    }
    public override string GetSummary () {
        if (typeOfFinish == finishType.PRE_BOSS_FINISH) {
            return "Finishing the game before boss";
        } else {
            return "Finishing the game after boss";
        }
    }
    public override Color GetButtonColor () {
        return new Color32 (45, 198, 234, 255);
    }
}