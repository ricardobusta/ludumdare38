using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the screen where the team logos show up.
/// </summary>
public class LogosScreenManager : MonoBehaviour {

  float timer = 2;

  public string gotoScene;

  /// <summary>
  /// 
  /// </summary>
  void Update() {
    timer -= Time.deltaTime;

    if (timer <= 0 || Input.GetButton("P1_Fire") || Input.GetButton("P2_Fire") || Input.GetButton("Submit")) {
      SceneManager.LoadScene(gotoScene);
    }
  }
}
