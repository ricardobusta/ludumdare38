using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Template for any type of player options. 
/// </summary>
abstract public class OptionGeneric : MonoBehaviour {
  public string playerPrefsValue = "prefOption";
  public string optionName = "option";

  public abstract void SetDefault();

  /// <summary>
  /// If there is no option set, it will initialize it with default values.
  /// </summary>
  public void Initialize() {
    if (!PlayerPrefs.HasKey(playerPrefsValue) || OutOfRange()) {
      SetDefault();
    }
  }

  public abstract bool OutOfRange();
}
