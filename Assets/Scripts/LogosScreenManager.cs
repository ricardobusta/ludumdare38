using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LogosScreenManager : MonoBehaviour {

  float timer = 2;

  void Update() {
    timer -= Time.deltaTime;

    if (timer <= 0 || Input.GetButton("P1_Fire") || Input.GetButton("P2_Fire") || Input.GetButton("Submit")) {
      SceneManager.LoadScene("title_screen");
    }
  }
}
