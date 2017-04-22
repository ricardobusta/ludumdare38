using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeybind {
  // Representa os keybinds de um player
  public string axis_h;
  public string axis_v;
  public KeyCode shoot;
  public KeyCode jump;
  public KeyCode dash;
  public KeyCode duck;

  public PlayerKeybind(string axis_h, string axis_v, KeyCode shoot, KeyCode jump, KeyCode dash) {
    this.axis_h = axis_h;
    this.axis_v = axis_v;
    this.shoot = shoot;
    this.jump = jump;
    this.dash = dash;
  }
}

public class Player : MonoBehaviour {

  // FIXME - Arrumar um lugar melhor pra guardar esses dados
  static PlayerKeybind[] keys = {
    new PlayerKeybind("P1_Horizontal","P1_Vertical",KeyCode.U,KeyCode.I,KeyCode.Y),
    new PlayerKeybind("P2_Horizontal","P2_Vertical",KeyCode.Keypad0,KeyCode.KeypadPeriod,KeyCode.KeypadEnter)
  };

  // Qual o número desse player
  public int playerN;

  Animator animator;
  Mobile mobile;

  // Player está no chão?
  bool onGround = false;
  bool goDown = true;
  bool ducking = false;

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

  Collider2D collider;
  Collider2D[] obstacles = new Collider2D[10];

  private void Awake() {
    mobile = GetComponent<Mobile>();
    animator = GetComponent<Animator>();
    collider = GetComponent<Collider2D>();
  }

  private void Start() {

  }

  void HandleCollision() {
    int n = collider.GetContacts(obstacles);
    for (int i = 0; i < n; i++) {
      ColliderDistance2D d = collider.Distance(obstacles[i]);
      var v = d.pointA - d.pointB;
      mobile.fromCartesian(mobile.toCartesian() - v);
    }
  }

  // Update is called once per frame
  void Update() {
    float h = Input.GetAxis(keys[playerN].axis_h);
    float v = Input.GetAxis(keys[playerN].axis_v);
    bool jumping = Input.GetKeyDown(keys[playerN].jump) || (v > 0);

    if (currentFireCD > 0) {
      // Se o cooldown de tiro é positivo, decremente
      currentFireCD -= Time.deltaTime;
    } else if (Input.GetKey(keys[playerN].shoot)) {
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
    ducking = false;
    if (onGround && h == 0 && v < 0) {
      ducking = true;
    }
    if (onGround) {
      // // Se está no chão, deixe o personagem no chão
      animator.SetBool("jumping", false);
      vSpeed = 0;
      mobile.radius = rPlaneta + playerHeightOffset;
      goDown = false;

      if (jumping) {
        ducking = false;
        animator.SetBool("jumping", true);
        vSpeed = maxVSpeed;
        onGround = false;
      }
      //                    << TODO arrumar esses valores de pulo aqui                 >>
    } else if (goDown || (Mathf.Abs(mobile.radius) >= rPlaneta + 2 * playerHeightOffset)) {
      // Senão, aplique gravidade
      vSpeed -= gravity;
      goDown = true;
    }
    animator.SetFloat("vertical_speed", vSpeed);
    animator.SetBool("ducking", ducking);

    // Gravide aplica quando o botão solta
    if (Input.GetKeyUp(keys[playerN].jump)) {
      goDown = true;
    }

    // Cálculo de posição vertical e horizontal
    mobile.radius += vSpeed * Time.deltaTime;
    //if (h == 1) {
    mobile.Move(h * speed);
    if (h != 0 || vSpeed != 0) {
      HandleCollision();
    }
    //}
    // Animar na horizontal se ele estiver se movendo
    animator.SetBool("horizontal_moving", Mathf.Abs(h) > 0);

    //Debug.LogFormat("Player{5}: p({0},{1}) v({4},{3}) {2}",mobile.angle, mobile.radius, onGround,vSpeed,h * speed, playerN);
  }
}
