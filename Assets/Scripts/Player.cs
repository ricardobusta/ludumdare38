using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeybind {
  // Representa os keybinds de um player
  public string axis;
  public KeyCode shoot;
  public KeyCode jump;

  public PlayerKeybind(string axis, KeyCode shoot, KeyCode jump) {
    this.axis = axis;
    this.shoot = shoot;
    this.jump = jump;
  }
}

public class Player : MonoBehaviour {

  // FIXME - Arrumar um lugar melhor pra guardar esses dados
  static PlayerKeybind[] keys = {
    new PlayerKeybind("P1_Horizontal",KeyCode.U,KeyCode.I),
    new PlayerKeybind("P2_Horizontal",KeyCode.Keypad0,KeyCode.KeypadPeriod)};

  // Qual o número desse player
  public int playerN;

  Mobile mobile;

  // Player está no chão?
  bool onGround = false;
  bool goDown = true;

  // FIXME - Arrumar um lugar melhor pra guardar esse raio
  const float rPlaneta = 2;

  public float speed = 90;
  public float maxVSpeed = 3;
  public float gravity = 0.2f;

  // Essa é a velocidade atual vertical
  private float vSpeed = 0;

  public float fireCD = 1;
  float currentFireCD = 0;

  public float playerHeightOffset = 0.6f;

  private void Awake() {
    mobile = GetComponent<Mobile>();
  }

  private void Start() {

  }

  // Update is called once per frame
  void Update () {
    float h = Input.GetAxis(keys[playerN].axis);

    if (currentFireCD > 0) {
      // Se o cooldown de tiro é positivo, decremente
      currentFireCD -= Time.deltaTime;
    } else if (Input.GetKeyDown(keys[playerN].shoot)) {
      // Senão, deixe o jogador atirar
      Bullet b = GameManager.Instance().GetFreeBullet();
      if (b != null) {
        b.gameObject.SetActive(true);
        b.Activate(mobile);
        currentFireCD = fireCD;
      }
    }

    // Está no chão se o raio for menor igual que o planeta + altura do jogador
    onGround = (Mathf.Abs(mobile.radius) <= rPlaneta + playerHeightOffset);

    if (onGround) {
      // // Se está no chão, deixe o personagem no chão
      vSpeed = 0;
      mobile.radius = rPlaneta + playerHeightOffset;
      goDown = false;

      if (Input.GetKeyDown(keys[playerN].jump)) {
        vSpeed = maxVSpeed;
        onGround = false;
      }
    //                    << TODO arrumar esses valores de pulo aqui                 >>
    } else if (goDown || (Mathf.Abs(mobile.radius) >= rPlaneta + 2*playerHeightOffset)) {
      // Senão, aplique gravidade
      vSpeed -= gravity;
      goDown = true;
    }
  
    // Gravide aplica quando o botão solta
    if (Input.GetKeyUp(keys [playerN].jump)) {
      goDown = true;
    }

    // Cálculo de posição vertical e horizontal
    mobile.radius += vSpeed * Time.deltaTime;
    mobile.Move(h * speed);


    //Debug.LogFormat("Player{5}: p({0},{1}) v({4},{3}) {2}",mobile.angle, mobile.radius, onGround,vSpeed,h * speed, playerN);
	}
}
