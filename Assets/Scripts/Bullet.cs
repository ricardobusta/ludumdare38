﻿using System.Collections;
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
    GameManager gm = GameManager.Instance();
    if (gm.gameOver) { return; }

    mobile.Move(speed);


    if (stepCol.IsTouchingLayers(LayerMask.GetMask("Player"))) {
      foreach (Player p in GameManager.Instance().players) {
        if (stepCol.IsTouching(p.GetComponent<Collider2D>())){
          var pCol = p.GetComponent<Collider2D>();
          var pMob = p.GetComponent<Mobile>();
          //ColliderDistance2D d = stepCol.Distance(pCol);
          //var v = d.pointB-d.pointA;
          var a = (stepCol.attachedRigidbody.worldCenterOfMass - pCol.attachedRigidbody.worldCenterOfMass).normalized;
          var angle = Mathf.Acos(Vector2.Dot(a, mobile.getNormal())) * Mathf.Rad2Deg;
          if (angle > 100) {
            pMob.fromCartesian(pMob.toCartesian() + mobile.getNormal()*0.06f);
            p.onSomething = mobile;
            p.vSpeed = 0;
            return;
          }
        }
      }
    }


    if (hurtCol.IsTouchingLayers(LayerMask.GetMask("Player"))) {
      print("!");
      foreach (Player p in GameManager.Instance().players) {
        if (hurtCol.IsTouching(p.GetComponent<Collider2D>())){
          p.playerLives--;
          gameObject.SetActive(false);

          mobile.direction = 0;
          mobile.radius = 0;
          mobile.angle = 0;

          if (p.playerLives < 0) {
            gm.gameOver = true;
            gm.Finish();
          }
        }
      }
    }
  }

  public void SetColor(Color c) {
    GetComponent<SpriteRenderer>().color = c;
  }

  public void Activate(Mobile mob) {
    mobile.angle = mob.angle + (mob.direction * bulletAngleOffset / mob.radius);
    mobile.radius = mob.radius;
    speed = Mathf.Abs(speed) * mob.direction;
    mobile.refresh();
  }
}
