using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerControl {
  public string horizontal_axis;
  public string vertical_axis;
  public Dictionary<string, KeyCode> keyMap = new Dictionary<string, KeyCode>();

  public PlayerControl() {
    keyMap.Add("jump", KeyCode.Y);
    keyMap.Add("dash", KeyCode.U);
    keyMap.Add("attack", KeyCode.I);
    keyMap.Add("fire", KeyCode.O);
    keyMap.Add("up", KeyCode.UpArrow);
    keyMap.Add("down", KeyCode.DownArrow);
    keyMap.Add("left", KeyCode.LeftArrow);
    keyMap.Add("right", KeyCode.RightArrow);
  }

  public void LoadDefaultControl(int playerID) {
    horizontal_axis = "P" + playerID + "_Horizontal";
    vertical_axis = "P" + playerID + "_Vertical";
    switch (playerID) {
      case 1:
        keyMap["jump"] = KeyCode.Y;
        keyMap["dash"] = KeyCode.U;
        keyMap["attack"] = KeyCode.I;
        keyMap["fire"] = KeyCode.O;
        keyMap["up"] = KeyCode.W;
        keyMap["down"] = KeyCode.S;
        keyMap["left"] = KeyCode.A;
        keyMap["right"] = KeyCode.D;
        break;
      case 2:
        keyMap["jump"] = KeyCode.Keypad0;
        keyMap["dash"] = KeyCode.Keypad1;
        keyMap["attack"] = KeyCode.Keypad2;
        keyMap["fire"] = KeyCode.KeypadPeriod;
        keyMap["up"] = KeyCode.UpArrow;
        keyMap["down"] = KeyCode.DownArrow;
        keyMap["left"] = KeyCode.LeftArrow;
        keyMap["right"] = KeyCode.RightArrow;
        break;
      case 3:
        keyMap["jump"] = KeyCode.H;
        keyMap["dash"] = KeyCode.J;
        keyMap["attack"] = KeyCode.K;
        keyMap["fire"] = KeyCode.L;
        keyMap["up"] = KeyCode.G;
        keyMap["down"] = KeyCode.B;
        keyMap["left"] = KeyCode.V;
        keyMap["right"] = KeyCode.N;
        break;
      case 4:
        keyMap["jump"] = KeyCode.KeypadMultiply;
        keyMap["dash"] = KeyCode.KeypadDivide;
        keyMap["attack"] = KeyCode.Keypad9;
        keyMap["fire"] = KeyCode.Keypad7;
        keyMap["up"] = KeyCode.Keypad8;
        keyMap["down"] = KeyCode.Keypad5;
        keyMap["left"] = KeyCode.Keypad4;
        keyMap["right"] = KeyCode.Keypad6;
        break;
    }
  }

  public void SavePlayerControls(int playerID) {
    string pname = "P" + playerID + "_";

    PlayerPrefs.SetString(pname + "horizontal", horizontal_axis);
    PlayerPrefs.SetString(pname + "vertical", vertical_axis);

    List<string> keys = new List<string>();
    foreach (string k in keyMap.Keys) {
      keys.Add(k);
    }

    foreach (string k in keys) {
      PlayerPrefs.SetInt(pname + k, (int)keyMap[k]);
    }
  }

  public void LoadPlayerControls(int playerID) {
    string pname = "P" + playerID + "_";

    horizontal_axis = PlayerPrefs.GetString(pname + "horizontal");
    vertical_axis = PlayerPrefs.GetString(pname + "vertical");

    List<string> keys = new List<string>();
    foreach (string k in keyMap.Keys) {
      keys.Add(k);
    }

    foreach (var k in keys) {
      keyMap[k] = (KeyCode)PlayerPrefs.GetInt(pname + k);
    }
  }
}

public class CustomController : MonoBehaviour {

  PlayerControl[] control;

  static CustomController _instance;

  // Use this for initialization
  private void Awake() {
    _instance = this;
    control = new PlayerControl[4];

    for(int i = 0; i < 4; i++) {
      control[i] = new PlayerControl();
    }

    if (!PlayerPrefs.HasKey("P1_jump")) {
      print("don't have controls saved");
      for (int i = 0; i < 4; i++) {
        control[i].LoadDefaultControl(i + 1);
        control[i].SavePlayerControls(i + 1);
      }
    }
    for (int i = 0; i < 4; i++) {
      var x = control[i];
      control[i].LoadPlayerControls(i + 1);
    }
  }

  void Start() {
  }

  public static CustomController Instance() {
    return _instance;
  }

  public static bool GetKey(int PlayerID, string control) {
    return Input.GetKey(_instance.control[PlayerID - 1].keyMap[control]);
  }

  public static bool GetKeyDown(int PlayerID, string control) {
    return Input.GetKeyDown(_instance.control[PlayerID - 1].keyMap[control]);
  }

  public static bool GetKeyUp(int PlayerID, string control) {
    return Input.GetKeyUp(_instance.control[PlayerID - 1].keyMap[control]);
  }

  public static float GetAxisV(int PlayerID) {
    float axis = Input.GetAxis(_instance.control[PlayerID - 1].vertical_axis);
    float digital = (GetKey(PlayerID, "up") ? 1 : 0) - (GetKey(PlayerID, "down") ? 1 : 0);
    return Mathf.Clamp(axis + digital, -1.0f, 1.0f);
  }

  public static float GetAxisH(int PlayerID) {
    float axis = Input.GetAxis(_instance.control[PlayerID - 1].horizontal_axis);
    float digital = (GetKey(PlayerID, "right") ? 1 : 0) - (GetKey(PlayerID, "left") ? 1 : 0);
    return Mathf.Clamp(axis + digital, -1.0f, 1.0f);
  }

  public static bool GetAxisDownV(int PlayerID) {
    bool axis = Input.GetButtonDown(_instance.control[PlayerID - 1].horizontal_axis);
    bool digital = GetKeyDown(PlayerID, "up") || GetKeyDown(PlayerID, "down");
    return axis || digital;
  }

  public static bool GetAxisDownH(int PlayerID) {
    bool axis = Input.GetButtonDown(_instance.control[PlayerID - 1].horizontal_axis);
    bool digital = GetKeyDown(PlayerID, "left") || GetKeyDown(PlayerID, "right");
    return axis || digital;
  }

  public static bool GetAxisUpV(int PlayerID) {
    bool axis = Input.GetButtonUp(_instance.control[PlayerID - 1].vertical_axis);
    bool digital = GetKeyUp(PlayerID, "up") || GetKeyUp(PlayerID, "down");
    return axis || digital;
  }

  public static bool GetAxisUpH(int PlayerID) {
    bool axis = Input.GetButtonUp(_instance.control[PlayerID - 1].horizontal_axis);
    bool digital = GetKeyUp(PlayerID, "left") || GetKeyUp(PlayerID, "right");
    return axis || digital;
  }

  public static void SetKey(int playerID, string key, KeyCode newValue) {
    _instance.control[playerID - 1].keyMap[key] = newValue;
    string pname = "P" + playerID + "_";
    PlayerPrefs.SetInt(pname + key, (int)newValue);
  }
}
