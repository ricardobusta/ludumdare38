using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour {

  EventSystem eventSystem;

  GameObject previousSelected = null;

  public AudioSource music1;
  public AudioSource music2;
  public AudioClip startClip;
  public GameObject commonBG;

  public Image imageBlock;

  [Header("Options")]
  public Slider planetSizeSlider;
  public Text planetSizeValue;
  public Slider playerLivesSlider;
  public Text playerLivesValue;
  public Slider playerBulletsSlider;
  public Text playerBulletsValue;
  public Slider playerSlider;
  public Text playerValue;
  public Slider bulletSpeedSlider;
  public Text bulletSpeedValue;
  public Toggle nightModeToggle;
  public Toggle turnPlanetToggle;

  public GameObject[] screens;

  private void Start() {
    SetScreen("Main Menu");
    eventSystem = FindObjectOfType<EventSystem>();

    float f;
    int i;

    commonBG.SetActive(false);

    i = PlayerPrefs.GetInt("nightMode", 0);
    nightModeToggle.isOn = (i == 1);

    i = PlayerPrefs.GetInt("turnPlanetItem", 1);
    turnPlanetToggle.isOn = (i == 1);
    
    imageBlock.gameObject.SetActive(false);
  }

  public void StartGame() {
    music1.Stop();
    music2.Stop();
    music2.clip = startClip;
    music2.loop = false;
    music2.Play();
    StartCoroutine(StartGameRoutine());
    imageBlock.gameObject.SetActive(true);
    eventSystem.SetSelectedGameObject(null);
  }

  IEnumerator StartGameRoutine() {
    while (music2.isPlaying) {
      yield return new WaitForEndOfFrame();
    }
    SceneManager.LoadScene("game_scene");
  }

  public void Exit() {
    Application.Quit();
  }

  public void SetScreen(string scene) {
    foreach (GameObject screen in screens) {
      screen.SetActive(screen.name == scene);
      commonBG.SetActive(scene != "Main Menu");
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

  public void SetNightMode() {
    PlayerPrefs.SetInt("nightMode", nightModeToggle.isOn ? 1 : 0);
  }

  public void SetTurnPlanet() {
    PlayerPrefs.SetInt("turnPlanetItem", turnPlanetToggle.isOn ? 1 : 0);
  }
}
