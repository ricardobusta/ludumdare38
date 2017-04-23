using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour {

  EventSystem eventSystem;

  GameObject previousSelected = null;

  private void Start() {
    eventSystem = FindObjectOfType<EventSystem>();
  }

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
