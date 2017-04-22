using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

  public float speed;
  Mobile mobile;

  // Use this for initialization
  void Start () {
    mobile = GetComponent<Mobile>();
  }
	
	// Update is called once per frame
	void Update () {
    mobile.Move(speed);
	}
}
