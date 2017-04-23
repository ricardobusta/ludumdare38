using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour {
  // Update is called once per frame
  float timer = 1;

  public GameObject gameOverMenu;

  private void Start() {
    gameOverMenu.SetActive(false);
  }

  void Update() {
    timer -= Time.deltaTime;

    gameOverMenu.SetActive(timer <= 0);

    if (timer > 0) {
      return;
    }
  }

  public void Replay() {
    SceneManager.LoadScene("game_scene");
  }

  public void MainMenu() {
    SceneManager.LoadScene("title_screen");
  }
}
