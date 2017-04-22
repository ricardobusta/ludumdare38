using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobile : MonoBehaviour {

  public float r = 0;
  public float a = 0;

  // Use this for initialization
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    Vector3 p = new Vector3(r * Mathf.Cos(a * Mathf.Deg2Rad), r * Mathf.Sin(a * Mathf.Deg2Rad), 0);
    transform.position = p;
    transform.localRotation = Quaternion.Euler(0, 0, a);
  }
}
