using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler {
  public void OnPointerEnter(PointerEventData ped) {
    AudioManager.Instance().PlayFire();
  }

  public void OnPointerDown(PointerEventData ped) {
    AudioManager.Instance().PlayJump();
  }
}