using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobile : MonoBehaviour {

  public float radius = 0;
  public float angle = 0;
  public float direction = 1;

  // Use this for initialization
  void Start() {
    fromCartesian(transform.position);
  }

  public void Move(float speed) {
    if (speed != 0) {
      Vector3 s = transform.localScale;
      direction = Mathf.Sign(speed);
      s.x = direction * Mathf.Abs(s.x);
      transform.localScale = s;
    }
    
    angle += speed * Time.deltaTime;

    if(angle > 360) {
      angle -= 360;
    }

    if(angle < 0) {
      angle += 360;
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
    transform.localRotation = Quaternion.Euler(0, 0, -angle-90);
  }
}

//tá me ouvindo?