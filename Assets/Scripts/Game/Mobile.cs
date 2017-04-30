using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the physics of a mobile element around the planet.
/// </summary>
public class Mobile : MonoBehaviour {

  public float radius = 0;
  public float angle = 0;
  public float direction = 1;

  /// <summary>
  /// 
  /// </summary>
  private void Awake() {
    fromCartesian(transform.position);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="speedx"></param>
  /// <param name="speedy"></param>
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

  /// <summary>
  /// 
  /// </summary>
  /// <param name="x"></param>
  /// <param name="y"></param>
  public void fromCartesian(float x, float y) {
    radius = Mathf.Sqrt(x * x + y * y);
    angle = Mathf.Rad2Deg * -Mathf.Atan2(y, x);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="v"></param>
  public void fromCartesian(Vector3 v) {
    fromCartesian(v.x, v.y);
    refresh();
  }

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public Vector2 toCartesian() {
    return new Vector2(radius * Mathf.Cos(-angle * Mathf.Deg2Rad), radius * Mathf.Sin(-angle * Mathf.Deg2Rad));
  }

  /// <summary>
  /// 
  /// </summary>
  public void refresh() {
    Vector3 p = toCartesian();
    Quaternion rot = Quaternion.Euler(0, 0, -angle - 90);
    if (!float.IsNaN(rot.x + rot.y + rot.z + rot.w)) {
      transform.localRotation = rot;
    } else {
      print("QUATERNION BUG!!!");
    }
    transform.position = p;
  }

  // Update is called once per frame
  void Update() {
    refresh();
    //Debug.DrawRay(Vector2.zero, transform.position, Color.cyan);
  }
}