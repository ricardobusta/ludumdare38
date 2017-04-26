using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionCheckBox : OptionGeneric {
  public string playerPrefsValue = "prefOption";
  public string optionName = "option";
  public bool defaultValue = true;

  [Header("References")]
  public Text textName;
  public Toggle toggle;

  private void Start() {
    textName.text = optionName;
    bool b = (PlayerPrefs.GetInt(playerPrefsValue, defaultValue?1:0)==1);
    toggle.isOn = b;
  }

  public void ValueChanged() {
    bool b = toggle.isOn;
    PlayerPrefs.SetInt(playerPrefsValue, b?1:0);
  }

  public override void SetDefault() {
    toggle.isOn = defaultValue;
  }
}
