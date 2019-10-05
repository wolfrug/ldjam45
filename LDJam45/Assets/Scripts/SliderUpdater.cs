using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderUpdater : MonoBehaviour {

    public Text displayText_;
    public Slider slider_;

    public string formatString_ = "{0:P0}";

    void Start() {

        if (slider_ == null) {
            slider_ = GetComponent<Slider>();
        }
        slider_.onValueChanged.AddListener(delegate { setText(); });

    }

	public void setText() {
        float input = slider_.value;
        //Debug.Log("running");
        displayText_.text = string.Format(formatString_, input);

    }

}
