using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetVisualBuilder : MonoBehaviour {

  public Color[] colors;

  // Use this for initialization
  void Start() {
    GetComponent<SpriteRenderer>().color = colors[Random.Range(0, colors.Length)];
  }
}
