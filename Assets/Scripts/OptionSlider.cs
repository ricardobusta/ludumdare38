using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSlider : MonoBehaviour {
  public string playerPrefsValue = "prefOption";
  public string optionName = "option";
  public string suffix = "%";
  public int minValue = 50;
  public int defaultValue = 100;
  public int maxValue = 500;

  [Header("References")]
  public Text textName;
  public Text textValue;
  public Slider slider;

  private void Start() {
    textName.text = optionName;
    float f = PlayerPrefs.GetFloat(playerPrefsValue, defaultValue);
    textValue.text = f.ToString() + suffix;
    slider.minValue = minValue;
    slider.maxValue = maxValue;
    slider.value = f;
  }

  public void ValueChanged() {
    float v = slider.value;
    print(v);
    PlayerPrefs.SetFloat(playerPrefsValue, v);
    textValue.text = v.ToString() + suffix;
  }
}
