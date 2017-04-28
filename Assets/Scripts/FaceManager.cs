using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the player portrait status
/// </summary>
public class FaceManager : MonoBehaviour
{
  /// <summary>
  /// Vector including all player portrait states 
  /// </summary>
  
  //TODO: J: Trocar isso por um enum.
  public Sprite[] sprites;  
  /// <summary>
  /// Unity UI component for player portrait
  /// </summary>
  Image image;

  private void Start() {
    image = GetComponent<Image>();
  }
  /// <summary>
  /// Sets player portrait according to his health percentage
  /// </summary>
  /// <param name="p">Player health percentage, varying from 0.0 to 1.0.</param>
  public void SetHealthPercent(float p) {
    //J: Esses comentarios não deveriam ser necessários (i.e. nome dos sprites deveriam expressar os estados deles).
    //Full Health
    if(p >= 1) {
      image.sprite = sprites[0];
    //Small Wounds
    } else if(p > 0.5f) {
      image.sprite = sprites[1];
    //Grievous Wounds
    }else if(p > 0) {
      image.sprite = sprites[2];
    //RIP in Pepperoni
    } else {
      image.sprite = sprites[3];
    }
  }
}
