using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

  Mobile mobile;
  bool onGround = false;
  const float rPlaneta = 2;

  public float speed = 1;
  public float maxVSpeed = 3;
  public float gravity = 0.1f;
  private float vSpeed = 0;

  public float fireCD = 1;
  float currentFireCD = 0;

  private void Awake() {
    mobile = GetComponent<Mobile>();
  }

  private void Start() {

  }

  // Update is called once per frame
  void Update () {
    float h = Input.GetAxis("Horizontal");
    mobile.Move(h * speed);

    if (currentFireCD > 0) {
      currentFireCD -= Time.deltaTime;
    } else {
      //float fire = Input.GetAxis("Fire1");
      float fire = Input.GetKeyDown(KeyCode.Z)?1:0;
      if (fire > 0) {
        Bullet b = GameManager.Instance().GetFreeBullet();
        if (b != null) {
          b.gameObject.SetActive(true);
          b.Activate(mobile);
          currentFireCD = fireCD;
        }
      }
    }

    onGround = (Mathf.Abs(mobile.radius) <= rPlaneta + 0.6);

    if (onGround && Input.GetKeyDown(KeyCode.X)) {
      vSpeed = maxVSpeed;
      onGround = false;
    }

    if (onGround)
      vSpeed = 0;
    else
      vSpeed -= gravity;
    
    mobile.radius += vSpeed * Time.deltaTime;
    Debug.LogFormat("Player: p({0},{1}) v({4},{3}) {2}",mobile.angle, mobile.radius, onGround,vSpeed,h * speed);
	}
}
