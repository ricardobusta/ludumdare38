using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The bullet projectile.
/// </summary>
public class Bullet : MonoBehaviour {

  public float initialSpeed;
  public float speed;
  Mobile mobile;

  const float bulletAngleOffset = 60;

  Collider2D hurtCol, stepCol;

  public GameManager gm;

  /// <summary>
  /// 
  /// </summary>
  private void Awake() {
    mobile = GetComponent<Mobile>();
    var cols = GetComponents<Collider2D>();
    hurtCol = cols[0];
    stepCol = cols[1];
  }

  /// <summary>
  /// 
  /// </summary>
  private void Update() {
    gm = GameManager.Instance;
    if (gm.pause || gm.gameOver) { return; }
    mobile.Move(speed);
  }

  /// <summary>
  /// 
  /// </summary>
  private void FixedUpdate() {
    gm = GameManager.Instance;
    if (gm.pause || gm.gameOver) { return; }

    if (stepCol.IsTouchingLayers(LayerMask.GetMask("Player"))) {
      foreach (Player p in GameManager.Instance.players) {
        if (stepCol.IsTouching(p.GetComponent<Collider2D>())) {
          var pCol = p.GetComponent<Collider2D>();
          var pMob = p.GetComponent<Mobile>();
          //ColliderDistance2D d = stepCol.Distance(pCol);
          //var v = d.pointB-d.pointA;
          var a = (stepCol.attachedRigidbody.worldCenterOfMass - pCol.attachedRigidbody.worldCenterOfMass).normalized;
          var angle = Mathf.Acos(Vector2.Dot(a, transform.up)) * Mathf.Rad2Deg;
          if (angle > 100) {
            pMob.radius = mobile.radius + gm.playerHeightOffset + 0.2f;
            pMob.refresh();
            p.onSomething = this;
            p.vSpeed = 0;
            return;
          }
        }
      }
    }


    if (hurtCol.IsTouchingLayers(LayerMask.GetMask("Player"))) {
      foreach (Player p in GameManager.Instance.players) {
        if (p.Invulnerable()) { continue; }
        if (hurtCol.IsTouching(p.GetComponent<Collider2D>())) {
          p.TakeDamage();
          RemoveSelf();
        }
      }
    }
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="c"></param>
  public void SetColor(Color c) {
    GetComponent<SpriteRenderer>().color = c;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="s"></param>
  public void SetSprite(Sprite s) {
    GetComponent<SpriteRenderer>().sprite = s;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="mob"></param>
  /// <param name="shootPoint"></param>
  public void Activate(Mobile mob, GameObject shootPoint) {
    Vector2 pos = shootPoint.transform.position;
    mobile.fromCartesian(pos.x, pos.y);
    //mobile.angle = mob.angle + (mob.direction * bulletAngleOffset / mob.radius);
    //mobile.radius = mob.radius;
    mobile.direction = mob.direction;
    initialSpeed = GameManager.Instance.baseBulletSpeed;
    print(initialSpeed);
    speed = initialSpeed * mob.direction;
    mobile.refresh();
  }

  /// <summary>
  /// 
  /// </summary>
  public void RemoveSelf() {
    gameObject.SetActive(false);

    mobile.direction = 0;
    mobile.radius = 0;
    mobile.angle = 0;
    mobile.refresh();
  }
}
