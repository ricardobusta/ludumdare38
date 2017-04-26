using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour {
  public OptionGeneric[] options;
  
  public void SetDefaults() {
    foreach(OptionGeneric o in options) {
      o.SetDefault();
    }
  }
}
