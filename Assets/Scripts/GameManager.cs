using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

  static GameManager _instance;

  public Bullet bulletPrefab;

  List<Bullet> bulletPool = new List<Bullet>();

  public float planetRadius = 10;

  public Text winnerText;
  public Image gameOverImage;

  public bool gameOver = false;

  [HideInInspector]
  public Player[] players;

  public int bulletPoolSize = 10;

  public static GameManager Instance() {
    return _instance;
  }

  // Use this for initialization
  void Start() {
    _instance = this;

    players = FindObjectsOfType<Player>();

    for (int i = 0; i < bulletPoolSize; i++) {
      Bullet b = Instantiate(bulletPrefab);
      b.gameObject.SetActive(false);
      bulletPool.Add(b);
    }

    gameOverImage.gameObject.SetActive(false);
  }

  public Bullet GetFreeBullet() {
    for (int i = 0; i < bulletPoolSize; i++) {
      if (!bulletPool[i].gameObject.activeSelf) {
        return bulletPool[i];
      }
    }
    return null;
  }

  public void Finish() {
    gameOverImage.gameObject.SetActive(true);
    bool draw = true;
    foreach (Player p in players) {
      if (p.playerLives >= 0) {
        winnerText.text = "Player " + p.playerN + " wins!";
        winnerText.color = p.bulletColor;
        draw = false; 
      }
    }
    if (draw) {
      winnerText.text = "Draw Game!";
    }
  }

  public void Restart() {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
}
