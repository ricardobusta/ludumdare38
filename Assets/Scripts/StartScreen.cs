using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour {

  // Use this for initialization
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("P1_Fire") || Input.GetButtonDown("P2_Fire") || Input.GetKeyDown(KeyCode.KeypadEnter)) {
      SceneManager.LoadScene("game_scene");
    }
  }
}
