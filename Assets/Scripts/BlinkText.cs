using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkText : MonoBehaviour {

    public float upperLimit = 1;
    public float lowerLimit = 0.1f;
    public float fadeRatio = 0.95f;

    Text txt = null;
       // Use this for initialization
    void Start () {
        txt = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        
        if(txt.color.r < lowerLimit || txt.color.g < lowerLimit || txt.color.b < lowerLimit)
        {
            fadeRatio = 1 + fadeRatio;
        }
        else if (txt.color.r > upperLimit || txt.color.g > upperLimit || txt.color.b > upperLimit)
        {
            fadeRatio = 1 - fadeRatio;
        }

        txt.color = new Color(txt.color.r * fadeRatio, txt.color.g * fadeRatio, txt.color.b * fadeRatio);
    }
}
