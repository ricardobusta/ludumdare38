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

  public GameObject[] screens;

  private void Start() {
    SetScreen("Main Menu");
    eventSystem = FindObjectOfType<EventSystem>();

    float f;
    int i;

    commonBG.SetActive(false);

    f = PlayerPrefs.GetFloat("planetSize", 2.14f);
    planetSizeSlider.value = f;
    planetSizeValue.text = f.ToString() + "%";

    i = PlayerPrefs.GetInt("playerLives", 3);
    playerLivesSlider.value = i;
    playerLivesValue.text = i.ToString();

    i = PlayerPrefs.GetInt("playerBullets", 5);
    playerBulletsSlider.value = i;
    playerBulletsValue.text = i.ToString();

    i = PlayerPrefs.GetInt("noOfPlayers", 2);
    playerSlider.value = i;
    playerValue.text = i.ToString();

    f = PlayerPrefs.GetFloat("bulletSpeed", 100.0f);
    bulletSpeedSlider.value = f;
    bulletSpeedValue.text = f.ToString()+"%";

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

  public void SetPlanetSize() {
    float v = planetSizeSlider.value;
    PlayerPrefs.SetFloat("planetSize", v);
    planetSizeValue.text = v.ToString() + "%";
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

  public void SetPlayers() {
    int v = (int)Mathf.Round(playerSlider.value);
    PlayerPrefs.SetInt("noOfPlayers", v);
    playerValue.text = v.ToString();
  }

  public void SetBulletSpeed() {
    float v = bulletSpeedSlider.value;
    PlayerPrefs.SetFloat("bulletSpeed", v);
    bulletSpeedValue.text = v.ToString() + "%";
  }

  public void SetNightMode() {
    PlayerPrefs.SetInt("nightMode", nightModeToggle.isOn ? 1 : 0);
  }
}
