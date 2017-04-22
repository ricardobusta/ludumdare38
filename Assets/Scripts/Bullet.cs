using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

  public float speed;
  Mobile mobile;

  const float bulletAngleOffset = 2;

  private void Awake() {
    mobile = GetComponent<Mobile>();
  }
	
	// Update is called once per frame
	void Update () {
    mobile.Move(speed);
	}

  public void Activate(Mobile mob) {
    mobile.angle = mob.angle;
    mobile.radius = mob.radius;
    mobile.direction = mob.direction;
    speed = speed * mob.direction;
  }
}
