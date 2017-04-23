using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void OnCollisionEnter2D(Collision2D x) {
    if (x.gameObject.name.Contains("Player")) {
      var p = x.gameObject.GetComponent<Player>();
      p.TakeDamage(100);
    } else if (x.gameObject.name.Contains("Bullet")) {
      var b = x.gameObject.GetComponent<Bullet>();
      b.RemoveSelf();
    }
  }
}
