using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

  ContactFilter2D filter = new ContactFilter2D();
  Collider2D[] obstacles = new Collider2D[1];
  new Collider2D collider;

  void Start() {
    collider = GetComponent<Collider2D>();
    filter.SetLayerMask(LayerMask.GetMask("Player"));
  }

  private void Update() {
    if (collider.GetContacts(filter, obstacles) > 0) {
      transform.position = Vector3.Scale(transform.position, new Vector3(1, -1, 1));
      float s = Random.Range(5, 15);
      GameManager.Respawn(gameObject, s);
      GameManager.RotateScreen();
      gameObject.SetActive(false);
    }
  }
}
