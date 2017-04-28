using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages all the options widgets at once.
/// </summary>
public class OptionsManager : MonoBehaviour {
  public OptionGeneric[] options;

  /// <summary>
  /// 
  /// </summary>
  private void Start() {
    Initialize();
  }

  /// <summary>
  /// 
  /// </summary>
  public void Initialize() {
    foreach (OptionGeneric o in options) {
      o.Initialize();
    }
  }

  /// <summary>
  /// 
  /// </summary>
  public void SetDefaults() {
    foreach (OptionGeneric o in options) {
      o.SetDefault();
    }
  }
}
