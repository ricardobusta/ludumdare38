using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

  Mobile mobile;

  public float speed = 1;

  private void Awake() {
    mobile = GetComponent<Mobile>();
  }

	// Update is called once per frame
	void Update () {
    float h = Input.GetAxis("Horizontal");

    mobile.Move(h * speed * Time.deltaTime);

    float fire = Input.GetAxis("Fire1");
    if(fire > 0) {
      Bullet b = GameManager.Instance().GetFreeBullet();
      if (b != null) {
        b.Activate(mobile);
        b.gameObject.SetActive(true);
      }
    }
	}
}
