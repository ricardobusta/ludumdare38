using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to create visual diversity in the game planet.
/// </summary>
public class PlanetVisualBuilder : MonoBehaviour {

  public Color[] colors;

  /// <summary>
  /// 
  /// </summary>
  void Start() {
    GetComponent<SpriteRenderer>().color = colors[Random.Range(0, colors.Length)];
  }
}
