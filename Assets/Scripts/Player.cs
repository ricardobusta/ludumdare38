using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

  Mobile mobile;
  bool onGround = false;
  const float r_planeta = 3;

  public float speed = 20;
  public float max_vspeed = 3;
  public float gravity = 0.1f;
  private float vspeed = 0;

  	// Use this for initialization
	void Start () {
    mobile = GetComponent<Mobile>();
    mobile.fromCartesian (transform.position);
	}
	
	// Update is called once per frame
	void Update () {
    float h = Input.GetAxis("Horizontal");
    mobile.Move(h * speed * Time.deltaTime);

    onGround = (Mathf.Abs(mobile.radius) <= r_planeta + 0.6);

    if (onGround && Input.GetKeyDown(KeyCode.Space)) {
      vspeed = max_vspeed;
      onGround = false;
    }

    if (onGround)
      vspeed = 0;
    else
      vspeed -= gravity;
    
    mobile.radius += vspeed * Time.deltaTime;
    Debug.LogFormat("p({0},{1}) v({4},{3}) {2}",mobile.angle, mobile.radius, onGround,vspeed,h * speed);
	}
}
