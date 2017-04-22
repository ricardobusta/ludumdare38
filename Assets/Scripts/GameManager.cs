using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

  static GameManager _instance;

  public Bullet bulletPrefab;

  List<Bullet> bulletPool = new List<Bullet>();

  [HideInInspector]
  public Player[] players;

  public int bulletPoolSize = 10;

  public static GameManager Instance() {
    return _instance;
  }

  public Bullet GetFreeBullet() {
    for (int i = 0; i < bulletPoolSize; i++) {
      if (!bulletPool[i].gameObject.activeSelf) {
        return bulletPool[i];
      }
    }
    return null;
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
  }
}
