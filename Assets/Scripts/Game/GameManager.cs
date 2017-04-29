using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages the game itself. 
/// </summary>
public class GameManager : MonoBehaviour {

  static GameManager _instance;

  public Bullet bulletPrefab;

  List<Bullet> bulletPool = new List<Bullet>();

  public float planetRadius = 10;
  public float playerHeightOffset = 0.6f;

  public Text winnerText;
  public Image gameOverImage;

  public GameObject planet;
  public Camera mainCamera;

  public Color nightModeColor;

  public bool pause = false;

  public GameObject starsContainer;
  public GameObject[] stars;

  public bool gameOver = false;

  int playerCount;

  public MusicManager musicManager;

  public Player[] players;

  public float baseBulletSpeed;

  private void Awake() {
    Options.Load();
  }

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public static GameManager Instance() {
    return _instance;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="go"></param>
  /// <param name="seconds"></param>
  public static void Respawn(GameObject go, float seconds) {
    _instance.StartCoroutine(_instance.RespawnCoroutine(go, seconds));
  }

  /// <summary>
  /// 
  /// </summary>
  public static void RotateScreen() {
    _instance.StartCoroutine(_instance.RotateScreenCoroutine());
  }

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public IEnumerator RotateScreenCoroutine() {
    pause = true;
    float totalTime = 3;
    float time = 0;

    Vector3 cameraRot = mainCamera.transform.localRotation.eulerAngles;
    float startAngle = cameraRot.z;

    while (time < totalTime) {
      time += Time.deltaTime;
      float t = time / totalTime;
      cameraRot.z = Mathf.Lerp(startAngle, startAngle + 180, t);
      mainCamera.transform.localRotation = Quaternion.Euler(cameraRot);
      yield return new WaitForEndOfFrame();
    }
    cameraRot.z = startAngle + 180;
    mainCamera.transform.localRotation = Quaternion.Euler(cameraRot);
    pause = false;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="go"></param>
  /// <param name="seconds"></param>
  /// <returns></returns>
  public IEnumerator RespawnCoroutine(GameObject go, float seconds) {
    print(go.name);
    print(seconds);
    yield return new WaitForSeconds(seconds);
    print("ok");
    go.SetActive(true);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public bool CheckGameOver() {
    int alive_count = 0;
    bool uno = false;
    for (int i = 0; i < playerCount; i++) {
      if (players[i].playerLives > 0) {
        alive_count++;
        uno |= players[i].playerLives == 1;
      }
    }
    if (alive_count <= 1) {
      return true;
    }
    if (alive_count <= 2 && uno) {
      musicManager.SetPitch(1.5f);
    }
    return false;
  }

  //private void Update() {
  //  playerHeightOffset = (0.6f / 4) * planetRadius;
  //  planet.transform.localScale = Vector3.one * planetRadius / 2.14f;
  //  mainCamera.orthographicSize = 2.336f * planetRadius;
  //}

  // Use this for initialization
  /// <summary>
  /// 
  /// </summary>
  void Start() {
    bool nightMode = Options.nightMode;
    if (nightMode) {
      mainCamera.backgroundColor = nightModeColor;
    }

    planetRadius = 0.0214f * Options.planetSize;
    playerHeightOffset = 0.2f * planetRadius + 0.3f;
    planet.transform.localScale = Vector3.one * planetRadius / 2.14f;
    mainCamera.orthographicSize = 2.336f * planetRadius;
    _instance = this;

    baseBulletSpeed = 3 * Options.bulletSpeed;
    print(baseBulletSpeed);

    playerCount = Options.numberOfPlayers;
    float ang = 360.0f / playerCount;
    print(ang);
    for (int i = 0; i < players.Length; i++) {
      players[i].SetActive(i < playerCount);
      players[i].Position((ang * i) + 90);
    }

    int bullet = playerCount * Options.playerBullets;
    for (int i = 0; i < bullet; i++) {
      Bullet b = Instantiate(bulletPrefab);
      b.gameObject.SetActive(false);
      bulletPool.Add(b);
    }

    gameOverImage.gameObject.SetActive(false);

    starsContainer.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));
    starsContainer.transform.localScale = Vector3.one * planetRadius / 2.15f;

    foreach (GameObject g in stars) {
      g.transform.localScale = Vector3.one;
      g.transform.rotation = Quaternion.identity;
    }
  }

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public Bullet GetFreeBullet() {
    for (int i = 0; i < bulletPool.Count; i++) {
      if (!bulletPool[i].gameObject.activeSelf) {
        return bulletPool[i];
      }
    }
    return null;
  }

  /// <summary>
  /// 
  /// </summary>
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

  /// <summary>
  /// 
  /// </summary>
  public void StartScreenShake() {
    StartCoroutine(ScreenShake());
  }

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  IEnumerator ScreenShake() {
    Vector3 basicCamPos = -Vector3.back * 10;

    float timer = 0.5f;
    while (timer > 0) {
      timer -= Time.deltaTime;
      Vector3 x = Random.insideUnitCircle * 0.1f;
      mainCamera.transform.position = x - basicCamPos;
      yield return new WaitForEndOfFrame();
    }
    mainCamera.transform.position = Vector3.zero - basicCamPos;
  }

  /// <summary>
  /// 
  /// </summary>
  public void Restart() {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
}
