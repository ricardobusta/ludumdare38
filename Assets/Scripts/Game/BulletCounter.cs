using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the interface element that counts the number of bullets left for each player.
/// </summary>
public class BulletCounter : MonoBehaviour {

  public Text text;
  public GameObject[] bullets;
  public Image image;

  const float spacing = 50;

  public int bulletCount;

  //private void Update() {
  //  SetBulletCount(bulletCount);
  //}

  /// <summary>
  /// 
  /// </summary>
  /// <param name="count"></param>
  public void SetBulletCount(int count) {
    if (count > bullets.Length) {
      image.gameObject.SetActive(true);
      text.text = "x" + count.ToString();
      bullets[0].SetActive(true);
      for (int i = 1; i < bullets.Length; i++) {
        bullets[i].SetActive(false);
      }
    } else {
      image.gameObject.SetActive(false);
      for (int i = 0; i < bullets.Length; i++) {
        bullets[i].SetActive(i < count);
        bullets[i].transform.localPosition = new Vector3(i * spacing, 0, 0);
      }
    }
  }
}
