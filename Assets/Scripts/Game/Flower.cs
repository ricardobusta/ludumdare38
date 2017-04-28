using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Flower : MonoBehaviour {
  /// <summary>
  /// 
  /// </summary>
  /// <param name="x"></param>
  void OnCollisionEnter2D(Collision2D x) {
    if (x.gameObject.name.Contains("Player")) {
      var p = x.gameObject.GetComponent<Player>();
      p.TakeDamage(1);
    } else if (x.gameObject.name.Contains("Bullet")) {
      var b = x.gameObject.GetComponent<Bullet>();
      //var bm = x.gameObject.GetComponent<Mobile>();
      //var p = GetComponentInParent<Player>();
      var pm = GetComponentInParent<Mobile>();
      if (b.isActiveAndEnabled) {
        //if (pm.angle > bm.angle) {
        //b.RemoveSelf();
        //p.ammoLeft++;
        b.speed = Mathf.Abs(b.speed) * pm.direction;
        print("GET");
        /*} else {
          print("FAIL");
        }*/
      }
    }
  }
}
