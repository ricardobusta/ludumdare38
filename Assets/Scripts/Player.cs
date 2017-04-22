using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

  Mobile mobile;

  public float speed = 30000;

  	// Use this for initialization
	void Start () {
    mobile = GetComponent<Mobile>();
    mobile.fromCartesian (transform.position);
	}
	
	// Update is called once per frame
	void Update () {
    float h = Input.GetAxis("Horizontal");

    Debug.Log(h + " " + speed + " " + Time.deltaTime);

    mobile.Move(h * speed * Time.deltaTime);
	}
}
