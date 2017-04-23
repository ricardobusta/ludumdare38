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
  public float playerHeightOffset = 0.6f;

  public Text winnerText;
  public Image gameOverImage;

  public GameObject planet;
  public Camera realCamera;

  public GameObject starsContainer;
  public GameObject[] stars;

  public bool gameOver = false;

  int playerCount;

  public Player[] players;

  public static GameManager Instance() {
    return _instance;
  }

  public bool CheckGameOver() {
    int alive_count = 0;
    for(int i = 0; i < playerCount; i++) { 
      if (players[i].playerLives > 0) {
        alive_count++;
      }
    }
    if (alive_count <= 1) {
      return true;
    }
    return false;
  }

  //private void Update() {
  //playerHeightOffset = (0.6f/4) * planetRadius;
  //planet.transform.localScale = Vector3.one * planetRadius / 2.14f;
  //mainCamera.orthographicSize = 2.336f * planetRadius;
  //}

  // Use this for initialization
  void Start() {
    planetRadius = PlayerPrefs.GetFloat("planetSize", 2);
    playerHeightOffset = 0.2f * planetRadius + 0.3f;
    planet.transform.localScale = Vector3.one * planetRadius / 2.14f;
    realCamera.orthographicSize = 2.336f * planetRadius;
    _instance = this;

    playerCount = PlayerPrefs.GetInt("noOfPlayers", 2);
    float ang = 360.0f / playerCount;
    print(ang);
    for (int i = 0; i < players.Length; i++) {
      players[i].gameObject.SetActive(i < playerCount);
      players[i].Position(ang * i);
    }

    int bullet = playerCount * PlayerPrefs.GetInt("playerBullets", 5);
    for (int i = 0; i < bullet; i++) {
      Bullet b = Instantiate(bulletPrefab);
      b.gameObject.SetActive(false);
      bulletPool.Add(b);
    }

    gameOverImage.gameObject.SetActive(false);

    starsContainer.transform.localRotation = Quaternion.Euler(0, 0,Random.Range(0.0f,360.0f)) ;
    starsContainer.transform.localScale = Vector3.one * planetRadius / 2.15f;

    foreach(GameObject g in stars) {
      g.transform.localScale = Vector3.one;
      g.transform.rotation = Quaternion.identity;
    }
  }

  public Bullet GetFreeBullet() {
    for (int i = 0; i < bulletPool.Count; i++) {
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
      if (p.playerLives > 0) {
        winnerText.text = "Player " + p.playerN + " wins!";
        winnerText.color = p.bulletColor;
        draw = false;
        return;
      }
    }
    if (draw) {
      winnerText.text = "Draw Game!";
    }
  }

  public void StartScreenShake() {
    StartCoroutine(ScreenShake());
  }

  IEnumerator ScreenShake() {
    Vector3 basicCamPos = -Vector3.back * 10;

    float timer = 0.5f;
    while (timer > 0) {
      timer -= Time.deltaTime;
      Vector3 x = Random.insideUnitCircle;
      realCamera.transform.position = x - basicCamPos;
      yield return new WaitForEndOfFrame();
    }
    realCamera.transform.position = Vector3.zero - basicCamPos;
  }

  public void Restart() {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
}
