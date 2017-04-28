using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A boolean option interface element. 
/// </summary>
public class OptionCheckBox : OptionGeneric {
  public bool defaultValue = true;

  [Header("References")]
  public Text textName;
  public Toggle toggle;

  /// <summary>
  /// 
  /// </summary>
  private void Start() {
    textName.text = optionName;
    bool b = (PlayerPrefs.GetInt(playerPrefsValue, defaultValue?1:0)==1);
    toggle.isOn = b;
  }

  /// <summary>
  /// 
  /// </summary>
  public void ValueChanged() {
    bool b = toggle.isOn;
    PlayerPrefs.SetInt(playerPrefsValue, b?1:0);
  }

  /// <summary>
  /// 
  /// </summary>
  public override void SetDefault() {
    toggle.isOn = defaultValue;
  }
}
