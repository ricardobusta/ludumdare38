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

  // Update is called once per frame
  void Update() {
    Vector3 p = new Vector3(radius * Mathf.Cos(-angle * Mathf.Deg2Rad), radius * Mathf.Sin(-angle * Mathf.Deg2Rad), 0);
    transform.position = p;
    transform.localRotation = Quaternion.Euler(0, 0, -angle);
  }
}

//tá me ouvindo?