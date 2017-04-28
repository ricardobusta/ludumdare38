using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls a spinning object with given speed (in degrees per second).
/// </summary>
public class SpinThing : MonoBehaviour {
  public float speed;

  float angle = 0;

  // Update is called once per frame
  void Update() {
    angle += speed * Time.deltaTime;
    transform.localRotation = Quaternion.Euler(0, 0, angle);
  }
}
