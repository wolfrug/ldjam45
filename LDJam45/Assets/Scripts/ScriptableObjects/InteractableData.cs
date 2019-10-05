using System.Collections;
using UnityEngine;

public enum InteractableColor {
    RED = 1000,
    BLUE = 2000,
    GREEN = 3000,
    NONE = 0000
}

[CreateAssetMenu (fileName = "Data", menuName = "InteractableData", order = 1)]
public class InteractableData : ScriptableObjectBase {

    public InteractableColor type;

}