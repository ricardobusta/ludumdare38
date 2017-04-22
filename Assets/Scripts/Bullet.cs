using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

  public float speed;
  Mobile mobile;

  const float bulletAngleOffset = 30;

  Collider2D hurtCol, stepCol;

  private void Awake() {
    mobile = GetComponent<Mobile>();
    var cols = GetComponents<Collider2D>();
    hurtCol = cols [0];
    stepCol = cols [1];
  }

  // Update is called once per frame
  void Update() {
    mobile.Move(speed);


    if (stepCol.IsTouchingLayers(LayerMask.GetMask("Player"))) {
      foreach (Player p in GameManager.Instance().players) {
        if (stepCol.IsTouching(p.GetComponent<Collider2D>())){
          var pCol = p.GetComponent<Collider2D>();
          var pMob = p.GetComponent<Mobile>();
          ColliderDistance2D d = stepCol.Distance(pCol);
          var v = d.pointB-d.pointA;
          var angle = Mathf.Acos(Vector2.Dot(v.normalized, mobile.getNormal())) * Mathf.Rad2Deg;
          pMob.fromCartesian(pMob.toCartesian() - v*2f);
          if (angle > 100) {
            p.onSomething = true;
            p.vSpeed = 0;
            return;
          }
        }
      }
    }


    if (hurtCol.IsTouchingLayers(LayerMask.GetMask("Player"))) {
      foreach (Player p in GameManager.Instance().players) {
        if (hurtCol.IsTouching(p.GetComponent<Collider2D>())){
          Debug.LogFormat("{0}",p.name);
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
