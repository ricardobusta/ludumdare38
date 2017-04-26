using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobile : MonoBehaviour {

  public float radius = 0;
  public float angle = 0;
  public float direction = 1;

  private void Awake() {
    fromCartesian(transform.position);
  }

  // Use this for initialization
  void Start() {

  }

  public void Move(float speedx, float speedy = 0) {
    if (speedx != 0) {
      Vector3 s = transform.localScale;
      direction = Mathf.Sign(speedx);
      s.x = -direction * Mathf.Abs(s.x);
      transform.localScale = s;
    }

    angle += speedx * Time.deltaTime / radius;
    radius += speedy * Time.deltaTime;

    if (angle > 360) {
      angle -= 360;
    }

    if (angle < 0) {
      angle += 360;
    }
    //rb.velocity = speed;
  }

  public void fromCartesian(float x, float y) {
    radius = Mathf.Sqrt(x * x + y * y);
    angle = Mathf.Rad2Deg * -Mathf.Atan2(y, x);
  }

  public void fromCartesian(Vector3 v) {
    fromCartesian(v.x, v.y);
    refresh();
  }

  public Vector2 toCartesian() {
    return new Vector2(radius * Mathf.Cos(-angle * Mathf.Deg2Rad), radius * Mathf.Sin(-angle * Mathf.Deg2Rad));
  }

  //public Vector2 getNormal() {
  //  var o = toCartesian();
  //  radius += 1;
  //  o = toCartesian() - o;
  //  radius -= 1;
  //  return o.normalized;
  //}

  public void refresh() {
    Vector3 p = toCartesian();
    transform.localRotation = Quaternion.Euler(0, 0, -angle - 90);
    transform.position = p;
  }

  // Update is called once per frame
  void Update() {
    refresh();
    //Debug.DrawRay(Vector2.zero, transform.position, Color.cyan);
  }
}