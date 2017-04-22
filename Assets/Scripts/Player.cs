using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeybind {
  public static string GetHorizontal(int playerID) {
    return "P" + (playerID + 1) + "_Horizontal";
  }
  public static string GetVertical(int playerID) {
    return "P" + (playerID + 1) + "_Vertical";
  }
  public static string GetJump(int playerID) {
    return "P" + (playerID + 1) + "_Jump";
  }
  public static string GetFire(int playerID) {
    return "P" + (playerID + 1) + "_Fire";
  }
  public static string GetDash(int playerID) {
    return "P" + (playerID + 1) + "_Dash";
  }
}

public class Player : MonoBehaviour {
  // Qual o número desse player
  public int playerN;

  Animator animator;
  Mobile mobile;

  // Player está no chão?
  public bool onGround = false;
  bool goDown = true;
  bool ducking = false;

  public float speed = 90;
  public float maxVSpeed = 3;
  public float gravity = 0.2f;

  // Essa é a velocidade atual vertical
  private float vSpeed = 0;

  public float fireCD = 1;
  float currentFireCD = 0;

  public float playerHeightOffset = 0.6f;

  new Collider2D collider;
  Collider2D[] obstacles = new Collider2D[10];

  private void Awake() {
    mobile = GetComponent<Mobile>();
    animator = GetComponent<Animator>();
    collider = GetComponent<Collider2D>();
  }

  private void Start() {

  }

  void HandleCollision() {
    if (collider.GetContacts(obstacles) > 0) {
      ColliderDistance2D d = collider.Distance(obstacles[0]);
      //Debug.DrawLine(d.pointA, d.pointB);
      var v = d.pointA - d.pointB;
      //print(mobile.toCartesian() - v);
      Debug.DrawRay(mobile.toCartesian(), -v, Color.magenta);
      mobile.fromCartesian(mobile.toCartesian() - v);
      onGround = false;
      goDown = true;
      var angle = Mathf.Acos(Vector2.Dot(v.normalized, obstacles[0].GetComponent<Mobile>().getNormal())) * Mathf.Rad2Deg;
      if (angle > 90.9) {
        vSpeed = 0;
      }
    }
  }

  // Update is called once per frame
  void Update() {
    float h = Input.GetAxisRaw(PlayerKeybind.GetHorizontal(playerN));
    float v = Input.GetAxisRaw(PlayerKeybind.GetVertical(playerN));


    bool jumping = Input.GetButtonDown(PlayerKeybind.GetJump(playerN)) || (Input.GetButtonDown(PlayerKeybind.GetVertical(playerN)) && v > 0);
    bool releaseJump = Input.GetButtonUp(PlayerKeybind.GetJump(playerN)) || (Input.GetButtonUp(PlayerKeybind.GetVertical(playerN)) && v > 0);
    if (jumping) {
      print(playerN);
    }

    float planetR = GameManager.Instance().planetRadius;

    if (currentFireCD > 0) {
      // Se o cooldown de tiro é positivo, decremente
      currentFireCD -= Time.deltaTime;
    } else if (Input.GetButton(PlayerKeybind.GetFire(playerN))) {
      // Senão, deixe o jogador atirar
      Bullet b = GameManager.Instance().GetFreeBullet();
      if (b != null) {
        b.gameObject.SetActive(true);
        b.Activate(mobile);
        currentFireCD = fireCD;
      }
    }

    // Está no chão se o raio for menor igual que o planeta + altura do jogador
    onGround = (Mathf.Abs(mobile.radius) <= planetR + playerHeightOffset);
    ducking = false;
    if (onGround && h == 0 && v < 0) {
      ducking = true;
    }
    if (onGround) {
      // // Se está no chão, deixe o personagem no chão
      animator.SetBool("jumping", false);
      vSpeed = 0;
      mobile.radius = planetR + playerHeightOffset;
      goDown = false;

      if (jumping) {
        ducking = false;
        animator.SetBool("jumping", true);
        vSpeed = maxVSpeed;
        onGround = false;
      }
      //                    << TODO arrumar esses valores de pulo aqui                 >>
    } else if (goDown || (Mathf.Abs(mobile.radius) >= planetR + 2 * playerHeightOffset)) {
      // Senão, aplique gravidade
      vSpeed -= gravity;
      goDown = true;
    }
    animator.SetFloat("vertical_speed", vSpeed);
    animator.SetBool("ducking", ducking);

    // Gravide aplica quando o botão solta
    if (releaseJump) {
      goDown = true;
    }

    // Cálculo de posição vertical e horizontal
    mobile.Move(h * speed, vSpeed);
    if (h * speed != 0 || vSpeed != 0)
      HandleCollision();
    //}
    // Animar na horizontal se ele estiver se movendo
    animator.SetBool("horizontal_moving", Mathf.Abs(h) > 0);

    //if (playerN == 1)
    //Debug.LogFormat("Player{5}: p({0},{1}) v({4},{3}) {2}",mobile.angle, mobile.radius, onGround,vSpeed,h * speed, playerN);
  }
}
