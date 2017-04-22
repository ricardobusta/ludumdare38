using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

  public float speed;
  Mobile mobile;

  const float bulletAngleOffset = 30;

  Collider2D col;

  private void Awake() {
    mobile = GetComponent<Mobile>();
    col = GetComponent<Collider2D>();
  }

  // Update is called once per frame
  void Update() {
    GameManager gm = GameManager.Instance();
    if (gm.gameOver) { return; }

    mobile.Move(speed);


    if (col.IsTouchingLayers(LayerMask.GetMask("Player"))) {
      foreach (Player p in gm.players) {
        if (col.IsTouching(p.GetComponent<Collider2D>())){
          p.playerLives--;
          gameObject.SetActive(false);

          mobile.direction = 0;
          mobile.radius = 0;
          mobile.angle = 0;


        }
      }
    }
  }

  public void Activate(Mobile mob) {
    mobile.angle = mob.angle + (mob.direction * bulletAngleOffset / mob.radius);
    mobile.radius = mob.radius;
    speed = Mathf.Abs(speed) * mob.direction;
    mobile.refresh();
  }
}
