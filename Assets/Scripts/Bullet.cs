using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

  public float speed;
  Mobile mobile;

  const float bulletAngleOffset = 50;

  Collider2D col;

  private void Awake() {
    mobile = GetComponent<Mobile>();
    col = GetComponent<Collider2D>();
    print("awake");
  }

  // Update is called once per frame
  void Update() {
    mobile.Move(speed);


    if (col.IsTouchingLayers(LayerMask.GetMask("Player"))) {
      foreach (Player p in GameManager.Instance().players) {
        if (col.IsTouching(p.GetComponent<Collider2D>())){
          print(p.name);
          gameObject.SetActive(false);

          mobile.direction = 0;
        }
      }
    }
  }

  public void Activate(Mobile mob) {
    mobile.angle = mob.angle + (mob.direction * bulletAngleOffset / mob.radius);
    mobile.radius = mob.radius;
    mobile.direction = mob.direction;
    speed = speed * mob.direction;
    print(mob.direction);
  }
}
