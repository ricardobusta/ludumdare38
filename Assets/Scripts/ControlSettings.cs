using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSettings : MonoBehaviour {

  public int playerID=1;

	public void SetPlayer(int id) {
    playerID = id;
  }
}
