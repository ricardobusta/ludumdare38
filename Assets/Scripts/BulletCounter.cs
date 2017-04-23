using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletCounter : MonoBehaviour {

  public Text text;
  public GameObject[] bullets;

  public int bulletCount;

  public void SetBulletCount(int count) {
    if(count > 6) {
      text.gameObject.SetActive(true);
      text.text = count.ToString();
      bullets[0].SetActive(true);
      for(int i = 1; i < bullets.Length; i++) {
        bullets[i].SetActive(false);
      }
    } else {
      text.gameObject.SetActive(false);
      for (int i = 1; i < bullets.Length; i++) {
        bullets[i].SetActive(i<count);
      }
    }
  }
}
