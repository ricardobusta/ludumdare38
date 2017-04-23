using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour {
  public void StartGame() {
    SceneManager.LoadScene("game_scene");
  }

  public void Options() {

  }

  public void Credits() {

  }

  public void Exit() {
    Application.Quit();
  }
}
