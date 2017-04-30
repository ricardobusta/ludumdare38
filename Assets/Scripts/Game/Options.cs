using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serializes the options to make it easier to access it through the gameplay.
/// </summary>
[System.Serializable]
public class Options {

  /// <summary>
  /// 
  /// </summary>
  public static void Load() {
    planetSize = PlayerPrefs.GetInt("planetSize", 100);
    playerLives = PlayerPrefs.GetInt("playerLives", 5);
    playerBullets = PlayerPrefs.GetInt("playerBullets", 6);
    numberOfPlayers = PlayerPrefs.GetInt("noOfPlayers", 2);
    bulletSpeed = PlayerPrefs.GetInt("bulletSpeed", 100);
    nightMode = (PlayerPrefs.GetInt("nightMode", 0) == 1);
    turnPlanetItem = (PlayerPrefs.GetInt("turnPlanetItem", 1) == 1);
    mounting = (PlayerPrefs.GetInt("playersMounting", 0) == 1);
    Debug.Log("Mounting: " + mounting);
  }

  public static float planetSize { get; private set; }
  public static int playerLives { get; private set; }
  public static int playerBullets { get; private set; }
  public static int numberOfPlayers { get; private set; }
  public static float bulletSpeed { get; private set; }
  public static bool nightMode { get; private set; }
  public static bool turnPlanetItem { get; private set; }
  public static bool mounting { get; private set; }
}
