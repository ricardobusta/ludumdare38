using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobile : MonoBehaviour {

  public float radius = 0;
  public float angle = 0;

  // Use this for initialization
  void Start() {

  }

  public void Move(float speed) {
    if (speed > 0) {

    }
  }

  public void fromCartesian(float x, float y) {
    radius = Mathf.Sqrt(x * x + y * y);
    angle = Mathf.Rad2Deg * -Mathf.Atan2(y, x);
  }

  public void fromCartesian(Vector3 v) {
    fromCartesian(v.x, v.y);
  }

  public Vector2 toCartesian() {
    return new Vector2(radius * Mathf.Cos(-angle * Mathf.Deg2Rad), radius * Mathf.Sin(-angle * Mathf.Deg2Rad));
  }

  // Update is called once per frame
  void Update() {
    Vector3 p = toCartesian();
    transform.position = p;
    transform.localRotation = Quaternion.Euler(0, 0, -angle);
  }
}

//tá me ouvindo?