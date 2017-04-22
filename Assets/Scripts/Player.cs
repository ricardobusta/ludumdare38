using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

  Mobile mobile;

  public float speed = 0.4f;

  	// Use this for initialization
	void Start () {
    mobile = GetComponent<Mobile>();
	}
	
	// Update is called once per frame
	void Update () {
    float h = Input.GetAxis("Horizontal");

    mobile.angle += h*speed;
	}
}
