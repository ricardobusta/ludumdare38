using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour {
  // Update is called once per frame
  float timer = 1;

  public GameObject gameOverMenu;
  public Text proTip;
  public EventSystem eventSystem;

  public string[] tips;

  private void Start() {
    gameOverMenu.SetActive(false);
    proTip.text = "Dev tip: " + tips[Random.Range(0, tips.Length)];
  }

  void Update() {
    timer -= Time.deltaTime;

    gameOverMenu.SetActive(timer <= 0);

    //if (Input.GetButton("P1_Fire") || Input.GetButton("P2_Fire")) {
    //  GameObject focus = eventSystem.currentSelectedGameObject;
    //  if (focus != null) {
    //    var pointer = new PointerEventData(EventSystem.current);
    //    ExecuteEvents.Execute(focus, pointer, ExecuteEvents.submitHandler);
    //  }
    //}
  }

  public void Replay() {
    SceneManager.LoadScene("game_scene");
  }

  public void MainMenu() {
    SceneManager.LoadScene("title_screen");
  }
}
