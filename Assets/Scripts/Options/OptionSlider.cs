using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSlider : OptionGeneric {
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
  static int i = 0;
  public void ValueChanged() {
    i++;
    print("valueChanged "+ optionName + i);
    float v = slider.value;
    PlayerPrefs.SetFloat(playerPrefsValue, v);
    textValue.text = v.ToString() + suffix;
  }

  public override void SetDefault() {
    slider.value = defaultValue;
  }
}
