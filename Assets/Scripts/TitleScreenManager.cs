using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour {

  EventSystem eventSystem;

  GameObject previousSelected = null;

  [Header("Options")]
  public Slider planetSizeSlider;
  public Text planetSizeValue;
  public Slider playerLivesSlider;
  public Text playerLivesValue;
  public Slider playerBulletsSlider;
  public Text playerBulletsValue;

  public GameObject[] screens;

  private void Start() {
    SetScreen("Main Menu");
    eventSystem = FindObjectOfType<EventSystem>();

    float f;
    int i;

    f = PlayerPrefs.GetFloat("planetSize", 2.14f);
    planetSizeSlider.value = f;
    planetSizeValue.text = f.ToString("0.00");

    i = PlayerPrefs.GetInt("playerLives", 3);
    playerLivesSlider.value = i;
    playerLivesValue.text = i.ToString();

    i = PlayerPrefs.GetInt("playerBullets", 5);
    playerBulletsSlider.value = i;
    playerBulletsValue.text = i.ToString();
  }

  public void StartGame() {
    SceneManager.LoadScene("game_scene");
  }

  public void Exit() {
    Application.Quit();
  }

  public void SetScreen(string scene) {
    foreach(GameObject screen in screens) {
      screen.SetActive(screen.name == scene);
    }
  }

  private void Update() {
    if (eventSystem.currentSelectedGameObject == null) {
      eventSystem.SetSelectedGameObject(previousSelected);
    }
    previousSelected = eventSystem.currentSelectedGameObject;

    if (Input.GetButton("P1_Fire") || Input.GetButton("P2_Fire")) {
      GameObject focus = eventSystem.currentSelectedGameObject;
      if (focus != null) {
        var pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(focus, pointer, ExecuteEvents.submitHandler);
      }
    }
  }

  public void SetPlanetSize() {
    float v = planetSizeSlider.value;
    PlayerPrefs.SetFloat("planetSize", v);
    planetSizeValue.text = v.ToString("0.00");
  }

  public void SetPlayerLives() {
    int v = (int)Mathf.Round(playerLivesSlider.value);
    PlayerPrefs.SetInt("playerLives", v);
    playerLivesValue.text = v.ToString();
  }

  public void SetPlayerBullets() {
    int v = (int)Mathf.Round(playerBulletsSlider.value);
    PlayerPrefs.SetInt("playerBullets", v);
    playerBulletsValue.text = v.ToString();
  }
}
