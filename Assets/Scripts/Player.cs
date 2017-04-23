using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeybind {
  public static string GetHorizontal(int playerID) {
    return "P" + (playerID) + "_Horizontal";
  }
  public static string GetHorizontalStick(int playerID) {
    return "P" + (playerID) + "_Horizontal_Stick";
  }
  public static float GetAllHorizontal(int playerID) {
    float tecla = Input.GetAxisRaw(PlayerKeybind.GetHorizontal(playerID));
    float stick = Input.GetAxisRaw(PlayerKeybind.GetHorizontalStick(playerID));
    if (Mathf.Abs(tecla) > Mathf.Abs(stick)) {
      return tecla;
    } else {
      return stick;
    }
  }
  public static string GetVertical(int playerID) {
    return "P" + (playerID) + "_Vertical";
  }
  public static string GetJump(int playerID) {
    return "P" + (playerID) + "_Jump";
  }
  public static string GetFire(int playerID) {
    return "P" + (playerID) + "_Fire";
  }
  public static string GetMelee(int playerID) {
    return "P" + (playerID) + "_Melee";
  }
  public static string GetDash(int playerID) {
    return "P" + (playerID) + "_Dash";
  }
}

public class Player : MonoBehaviour {
  // Qual o número desse player
  public int playerN;

  public Color bulletColor;

  public int playerLives = 3;

  public int ammoLeft = 5;

  Animator animator;
  Mobile mobile;

  // Player está no chão?
  public bool onGround = false;
  public Bullet onSomething = null;
  public bool goDown = true;
  bool ducking = false;

  public float speed = 90;
  public float maxVSpeed = 3;
  public float gravity = 0.2f;

  // Essa é a velocidade atual vertical
  public float vSpeed = 0;

  public float fireCD = 1;
  float currentFireCD = 0;

  public float meleeCD = 1;
  float currentMeleeCD = 0;

  public float dashCD = 1;
  float currentDashCD = 0;
  public float dashDuration = 0.5f;
  float currentDashDuration = -1;
  float dashDirection = 0;
  public float dashSpeedMultiplier = 4;

  public float playerHeightOffset = 0.6f;

  new Collider2D collider;
  ContactFilter2D playerFilter = new ContactFilter2D();
  Collider2D[] obstacles = new Collider2D[10];
  RaycastHit2D[] rayResults = new RaycastHit2D[2];

  private void Awake() {
    mobile = GetComponent<Mobile>();
    animator = GetComponent<Animator>();
    collider = GetComponent<Collider2D>();
  }

  private void Start() {
    playerFilter.SetLayerMask(-257);
    playerFilter.useLayerMask = true;
  }

  void HandleCollision() {
    if (collider.GetContacts(playerFilter, obstacles) > 0) {
      ColliderDistance2D d = collider.Distance(obstacles[0]);
      //Debug.DrawLine(d.pointA, d.pointB);
      var v = d.pointA - d.pointB;
      //print(mobile.toCartesian() - v);
      Debug.DrawRay(mobile.toCartesian(), -v, Color.magenta);
      mobile.fromCartesian(mobile.toCartesian() - v);
      onGround = false;
      goDown = true;
      var angle = Mathf.Acos(Vector2.Dot(v.normalized, obstacles[0].GetComponent<Mobile>().getNormal())) * Mathf.Rad2Deg;
      if (angle > 100) {
        vSpeed = 0;
      }
    }
  }

  void Shoot() {
    if (currentFireCD > 0) {
      // Se o cooldown de tiro é positivo, decremente
      currentFireCD -= Time.deltaTime;
    } else if (ammoLeft > 0 && Input.GetButton(PlayerKeybind.GetFire(playerN))) {
      // Senão, deixe o jogador atirar
      Bullet b = GameManager.Instance().GetFreeBullet();
      if (b != null) {
        ammoLeft--;
        b.Activate(mobile);
        b.SetColor(bulletColor);
        b.gameObject.SetActive(true);
        currentFireCD = fireCD;
        AudioManager.Instance().PlayFire();
      }
    }
  }

  void Melee() {
    if (currentMeleeCD > 0) {
      // Se o cooldown de tiro é positivo, decremente
      currentMeleeCD -= Time.deltaTime;
    } else if (Input.GetButton(PlayerKeybind.GetMelee(playerN))) {
      currentMeleeCD = meleeCD;
      AudioManager.Instance().PlayFire();
    }
  }

  float Dash() {
    // Durante o dash, o jogador se comporta de um jeito diferente
    if (currentDashDuration < 0) {
      if (currentDashCD > 0) {
        // Se o cooldown de dash é positivo, decremente
        currentDashCD -= Time.deltaTime;
      } else if (Input.GetButton(PlayerKeybind.GetDash(playerN))) {
        // Deixa jogador dashar
        currentDashCD = dashCD;
        currentDashDuration = dashDuration;
        dashDirection = mobile.direction;
      }
      animator.SetBool("dashing", false);
      return 0;
    }

    animator.SetBool("dashing", true);
    currentDashDuration -= Time.deltaTime;

    return dashDirection*dashSpeedMultiplier;
  }

  // Update is called once per frame
  void Update() {
    GameManager gm = GameManager.Instance();
    if (gm.gameOver) { return; }
    float planetR = gm.planetRadius;

    if (collider.Cast(-mobile.getNormal(), rayResults, planetR) == 0) {
      onSomething = null;
    }

    float h = PlayerKeybind.GetAllHorizontal(playerN);
    float v = Input.GetAxisRaw(PlayerKeybind.GetVertical(playerN));


    bool jumping = Input.GetButtonDown(PlayerKeybind.GetJump(playerN)) || (Input.GetButtonDown(PlayerKeybind.GetVertical(playerN)) && v > 0);
    bool releaseJump = Input.GetButtonUp(PlayerKeybind.GetJump(playerN)) || (Input.GetButtonUp(PlayerKeybind.GetVertical(playerN)) && Input.GetAxis(PlayerKeybind.GetVertical(playerN)) > 0);
    /*if (jumping) {
      print(playerN);
    }*/
      
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
      mobile.radius = planetR + playerHeightOffset - 1e-3f;
      goDown = false;

      if (jumping) {
        ducking = false;
        animator.SetBool("jumping", true);
        vSpeed = maxVSpeed;
        AudioManager.Instance().PlayJump();
        onGround = false;
      }
      //                    << TODO arrumar esses valores de pulo aqui                 >>
    } else if ((goDown || (Mathf.Abs(mobile.radius) >= planetR + 2 * playerHeightOffset)) && !onSomething) {
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

    float d = Dash();
    if (d != 0) {
      h = d;
    }

    // Cálculo de posição vertical e horizontal
    mobile.Move(h * speed, vSpeed);
    if (h * speed != 0 || vSpeed != 0)
      HandleCollision();
    //}
    // Animar na horizontal se ele estiver se movendo
    animator.SetBool("horizontal_moving", Mathf.Abs(h) > 0);

    if (onSomething && onSomething.isActiveAndEnabled) {
      var bMob = onSomething.GetComponent<Mobile>();
      mobile.angle += onSomething.speed * Time.deltaTime / bMob.radius * bMob.direction;
    }

    Shoot();

    Dash();
    //if (playerN == 1)
    //Debug.LogFormat("Player{5}: p({0},{1}) v({4},{3}) {2}",mobile.angle, mobile.radius, onGround,vSpeed,h * speed, playerN);
  }
}
