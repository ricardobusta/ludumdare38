using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Controls the interface element that counts the number of bullets left for each player.
/// </summary>
public class BulletCounter : MonoBehaviour {

  /// <summary>
  /// UI component with the bullets remaining text 'xN'
  /// </summary>
  public Text text;
  
  /// <summary>
  /// Bullet object themselves
  /// </summary>
  public GameObject[] bullets;

  /// <summary>
  /// J: No Idea :(
  /// </summary>
  public Image image;
  
  /// <summary>
  /// Padding to position the bullet in front of a player
  /// J: É um bocado errado usar essa classe para posicionar a bala no cenário. IMO, ela deveria só retornar uma bala para o jogador, e fazer todo o trabalho de gerenciamento de balas por baixo
  /// ie. método GameObject getBullet()
  /// </summary>
  const float spacing = 50;

  //Dead var
  //public int bulletCount;

  //private void Update() {
  //  SetBulletCount(bulletCount);
  //}

  /// <summary>
  /// Sets the on the UI and in the bullet repository the current count of bullets with a player.
  /// </summary>
  /// <param name="count">Current number of bullets with a player</param>
  public void SetBulletCount(int count) {
    
    if(count > bullets.Length) {
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
