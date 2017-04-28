using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 
/// </summary>
public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler {
  /// <summary>
  /// 
  /// </summary>
  /// <param name="ped"></param>
  public void OnPointerEnter(PointerEventData ped) {
    AudioManager.Instance().PlayFire();
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="ped"></param>
  public void OnPointerDown(PointerEventData ped) {
    AudioManager.Instance().PlayJump();
  }
}