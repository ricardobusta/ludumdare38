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

  public GameObject[] screens;

  private void Start() {
    SetScreen("Main Menu");
    eventSystem = FindObjectOfType<EventSystem>();

    commonBG.SetActive(false);
    
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
}
