using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Makes it easier to swap sprites at runtime.
/// </summary>
public class SpriteSwapper : MonoBehaviour {

  public float currentSprite;
  public Sprite[] spriteSheet;

  /// <summary>
  /// SpriteRenderer cache
  /// </summary>
  private SpriteRenderer spriteRenderer;

  /// <summary>
  /// 
  /// </summary>
  void Start() {
    this.spriteRenderer = GetComponent<SpriteRenderer>();
  }

  /// <summary>
  /// 
  /// </summary>
  private void LateUpdate() {
    // Swap out the sprite to be rendered by its name
    print(currentSprite);
    this.spriteRenderer.sprite = spriteSheet[(int)Mathf.Ceil(currentSprite)];
  }
}
