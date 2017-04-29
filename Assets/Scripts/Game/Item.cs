using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Item. Currently the only item is the turnplanet.
/// </summary>
public class Item : MonoBehaviour {

  ContactFilter2D filter = new ContactFilter2D();
  Collider2D[] obstacles = new Collider2D[1];
  Collider2D collider2d;

  /// <summary>
  /// 
  /// </summary>
  void Start() {
    bool itemEnabled = PlayerPrefs.GetInt("turnPlanetItem", 1) == 1;
    if (!itemEnabled) {
      print("itemDisabled");
      gameObject.SetActive(false);
    }


    collider2d = GetComponent<Collider2D>();
    filter.SetLayerMask(LayerMask.GetMask("Player"));
  }

  /// <summary>
  /// 
  /// </summary>
  private void Update() {
    if (collider2d.GetContacts(filter, obstacles) > 0) {
      transform.position = Vector3.Scale(transform.position, new Vector3(1, -1, 1));
      float s = Random.Range(5, 15);
      GameManager.Respawn(gameObject, s);
      GameManager.RotateScreen();
      gameObject.SetActive(false);
    }
  }
}
