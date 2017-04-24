using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletCounter : MonoBehaviour {

  public Text text;
  public GameObject[] bullets;
  public Image image;

  const float spacing = 50;

  public int bulletCount;

  //private void Update() {
  //  SetBulletCount(bulletCount);
  //}

  public void SetBulletCount(int count) {
    if(count > bullets.Length) {
      image.gameObject.SetActive(true);
      text.text = "x"+count.ToString();
      bullets[0].SetActive(true);
      for(int i = 1; i < bullets.Length; i++) {
        bullets[i].SetActive(false);
      }
    } else {
      image.gameObject.SetActive(false);
      for (int i = 0; i < bullets.Length; i++) {
        bullets[i].SetActive(i<count);
        bullets[i].transform.localPosition = new Vector3(i*spacing, 0, 0);
      }
    }
  }
}
