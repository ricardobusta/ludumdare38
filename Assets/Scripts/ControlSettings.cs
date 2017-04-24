//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//public class ControlSettings : MonoBehaviour {

//  public int playerID=1;

//  public GameObject overlay;
//  public Text overlayText;

//  public EventSystem eventSystem;

//  public Text jumpButtonText;
//  public Text dashButtonText;
//  public Text attackButtonText;
//  public Text fireButtonText;
//  public Text upButtonText;
//  public Text downButtonText;
//  public Text leftButtonText;
//  public Text rightButtonText;
//  public Text axisHButtonText;
//  public Text axisVButtonText;

//  bool selecting = false;
//  string keyname = "";

//  public void SetPlayer(int id) {
//    playerID = id;

//    string pname = "P" + id + "_";

//    jumpButtonText.text = ((KeyCode)PlayerPrefs.GetInt(pname + "jump")).ToString();
//    dashButtonText.text = ((KeyCode)PlayerPrefs.GetInt(pname + "dash")).ToString();
//    attackButtonText.text = ((KeyCode)PlayerPrefs.GetInt(pname + "attack")).ToString();
//    fireButtonText.text = ((KeyCode)PlayerPrefs.GetInt(pname + "fire")).ToString();
//    upButtonText.text = ((KeyCode)PlayerPrefs.GetInt(pname + "up")).ToString();
//    downButtonText.text = ((KeyCode)PlayerPrefs.GetInt(pname + "down")).ToString();
//    leftButtonText.text = ((KeyCode)PlayerPrefs.GetInt(pname + "left")).ToString();
//    rightButtonText.text = ((KeyCode)PlayerPrefs.GetInt(pname + "right")).ToString();
//  }

//  private void Start() {
//    overlay.SetActive(false);

//    SetPlayer(1);
//  }

//  public void SelectKeyClicked(string key) {
//    selecting = true;
//    keyname = key;
//    overlay.SetActive(true);

//    overlayText.text = "selecting " + keyname + " for P" + playerID;
//    eventSystem.gameObject.SetActive(false);
//  }

//  void OnGUI() {
//    if (!selecting) return;

//    if (selecting) {
//      if (Event.current.isKey && Event.current.type == EventType.KeyDown) {
//        selecting = false;
//        overlay.SetActive(false);
//        if (Event.current.keyCode != KeyCode.Escape) {
//          CustomController.SetKey(playerID, keyname, Event.current.keyCode);
//          SetPlayer(playerID);
//        }
//        eventSystem.gameObject.SetActive(true);
//      }
//    }
//  }
//}
