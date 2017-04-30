using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A slider that uses whole values.
/// </summary>
public class OptionSlider : OptionGeneric {
  public string suffix = "%";
  public int minValue = 50;
  public int defaultValue = 100;
  public int maxValue = 500;

  [Header("References")]
  public Text textName;
  public Text textValue;
  public Slider slider;

  /// <summary>
  /// 
  /// </summary>
  private void Start() {
    textName.text = optionName;
    float f = PlayerPrefs.GetInt(playerPrefsValue, defaultValue);
    textValue.text = f.ToString() + suffix;
    slider.minValue = minValue;
    slider.maxValue = maxValue;
    slider.value = f;
  }

  /// <summary>
  /// 
  /// </summary>
  public void ValueChanged() {
    int v = (int)slider.value;
    PlayerPrefs.SetInt(playerPrefsValue, v);
    textValue.text = v.ToString() + suffix;
  }

  /// <summary>
  /// 
  /// </summary>
  public override void SetDefault() {
    slider.value = defaultValue;
  }
}
