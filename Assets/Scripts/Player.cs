using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeybind {
  public static string GetHorizontal(int playerID) {
    return "P" + (playerID) + "_Horizontal";
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

  int maxPlayerLives;
  public int playerLives = 3;

  public int ammoLeft = 5;

  public BulletCounter bulletCounter;
  public FaceManager faceManager;
  public AudioClip hitSound;

  Animator animator;
  Mobile mobile;

  public GameObject shootPoint;

  // Player está no chão?
  public bool onGround = false;
  public Bullet onSomething = null;
  public bool goDown = true;
  bool ducking = false;
  bool doubleJumping = false;
  float doubleJumpingHeight = 0;

  public float maxHSpeed = 180;
  public float maxVSpeed = 3;
  private float gravity = 8f;
  private float airDrag = 3f;
  private float trueHSpeed = 0;

  // Essa é a velocidade atual
  public float vSpeed = 0;
  public float hSpeed = 0;

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

  public float invulnerabilityTime = 1;
  float invulnerability = 0;

  bool dead = false;

  new Collider2D collider;
  ContactFilter2D playerFilter = new ContactFilter2D();
  Collider2D[] obstacles = new Collider2D[10];
  RaycastHit2D[] rayResults = new RaycastHit2D[2];

  private GameManager gm;

  private void Awake() {
    mobile = GetComponent<Mobile>();
    animator = GetComponent<Animator>();
    collider = GetComponent<Collider2D>();
  }

  private void Start() {
    playerLives = PlayerPrefs.GetInt("playerLives", 3);
    maxPlayerLives = playerLives;
    ammoLeft = PlayerPrefs.GetInt("playerBullets", 5);
    bulletCounter.SetBulletCount(ammoLeft);
    playerFilter.SetLayerMask(LayerMask.GetMask("Player"));
    playerFilter.useLayerMask = true;
    animator.SetInteger("player_health", playerLives);
  }

  void HandleCollision() {
    if (collider.GetContacts(playerFilter, obstacles) > 0) {
      ColliderDistance2D d = collider.Distance(obstacles[0]);
      //Debug.DrawLine(d.pointA, d.pointB);
      var v = d.pointA - d.pointB;
      var p2 = obstacles[0].GetComponent<Player>();
      var p2m = obstacles[0].GetComponent<Mobile>();
      var n = transform.up;
      onGround = false;
      goDown = true;
      var angle = Vector2.Angle(v, n);
      if ((angle > 170) && (p2m.radius < mobile.radius)) {
        var reflect = Vector2.Reflect(v, n).normalized;
        reflect = Quaternion.Euler(0, 0, -mobile.direction * 60) * reflect;
        //Debug.DrawRay(mobile.toCartesian(), reflect*0.4f, Color.magenta);
        mobile.fromCartesian(mobile.toCartesian() + reflect * 0.4f);
        vSpeed = maxVSpeed;
        hSpeed = maxHSpeed * mobile.direction;
        AudioManager.Instance().PlayHitHead();
        p2.TakeDamage();
        //print("pow");
        //Debug.Break();
      } else {  
          //Debug.DrawRay(mobile.toCartesian(), -v, Color.magenta);
          mobile.fromCartesian(mobile.toCartesian() - v);
      }
    }
  }

  public bool Invulnerable() {
    return invulnerability > 0;
  }

  void Shoot() {
    if (currentFireCD > 0) {
      // Se o cooldown de tiro é positivo, decremente
      currentFireCD -= Time.deltaTime;
    } else if (ammoLeft > 0 && CustomController.GetKey(playerN, "fire")) {
      // Senão, deixe o jogador atirar
      animator.SetTrigger("fire");
      print("shoot!");
      currentFireCD = fireCD;
    }
  }

  void ActualShoot() {
    Bullet b = GameManager.Instance().GetFreeBullet();
    if (b != null) {
      ammoLeft--;
      bulletCounter.SetBulletCount(ammoLeft);
      b.Activate(mobile, shootPoint);
      if (onSomething && onSomething.isActiveAndEnabled) {
        var bMob = onSomething.GetComponent<Mobile>();
        float rate = 0.33f * mobile.radius / bMob.radius;
        b.speed = Mathf.Abs(b.initialSpeed) * mobile.direction + onSomething.speed * rate;
        //onSomething.speed -= Mathf.Abs(speed) * mobile.direction;
        //bMob.refresh();
      }
      b.SetColor(bulletColor);
      b.gameObject.SetActive(true);
      AudioManager.Instance().PlayFire();
    }
  }

  void Melee() {
    if (currentMeleeCD > 0) {
      // Se o cooldown de tiro é positivo, decremente
      currentMeleeCD -= Time.deltaTime;
    } else if (CustomController.GetKey(playerN, "attack")) {
      currentMeleeCD = meleeCD;
      animator.SetTrigger("attack");
      AudioManager.Instance().PlayAttack();
    }
  }

  float Dash() {
    // Durante o dash, o jogador se comporta de um jeito diferente
    if (currentDashDuration < 0) {
      if (currentDashCD > 0) {
        // Se o cooldown de dash é positivo, decremente
        currentDashCD -= Time.deltaTime;
      } else if (CustomController.GetKeyDown(playerN, "dash")) {
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

    return dashDirection * dashSpeedMultiplier;
  }

  public void SetActive(bool active) {
    faceManager.gameObject.SetActive(active);
    bulletCounter.gameObject.SetActive(active);
    gameObject.SetActive(active);
  }

  public void TakeDamage(int i = 1) {
    if (invulnerability > 0) {
      return;
    }
    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) {
      return;
    }
    invulnerability = invulnerabilityTime;
    GameManager.Instance().StartScreenShake();
    AudioManager.Play(hitSound);
    animator.SetTrigger("hit");
    playerLives -= i;
    animator.SetInteger("player_health", playerLives);
    float pl = (float)playerLives / (float)maxPlayerLives;
    faceManager.SetHealthPercent(pl);
    if (playerLives <= 0) {
      dead = true;
    }

    if (gm.CheckGameOver()) {
      gm.gameOver = true;
      gm.Finish();
    }
  }

  public void Position(float angle) {
    GameManager gm = GameManager.Instance();
    mobile.radius = gm.planetRadius + gm.playerHeightOffset;
    mobile.angle = angle - 90;
    mobile.refresh();
  }

  // Update is called once per frame
  void Update() {
    if (dead) {
      gameObject.SetActive(false);
      return;
    }
    gm = GameManager.Instance();
    if (gm.pause || gm.gameOver) { return; }

    if (invulnerability > 0) {
      invulnerability -= Time.deltaTime;
    }
    animator.SetBool("invulnerable", invulnerability > 0);
    float planetR = gm.planetRadius;

    if (collider.Cast(-mobile.getNormal(), rayResults, planetR) == 0) {
      onSomething = null;
    }

    float h = CustomController.GetAxisH(playerN);
    float v = CustomController.GetAxisV(playerN);


    bool jumping = CustomController.GetKeyDown(playerN, "jump") || (CustomController.GetAxisDownV(playerN) && v > 0);
    bool releaseJump = CustomController.GetKeyUp(playerN, "jump") || ((CustomController.GetAxisUpV(playerN) ));
    /*if (jumping) {
      print(playerN);
    }*/
    if (jumping) {
      print("jumping!");
    }
    if (releaseJump) {
      print("releaseJump");
    }

    // Está no chão se o raio for menor igual que o planeta + altura do jogador
    onGround = (Mathf.Abs(mobile.radius) <= planetR + gm.playerHeightOffset);
    ducking = false;
    if ((onGround || onSomething) && h == 0 && v < 0) {
      ducking = true;
    }

    if (onGround || onSomething) {
      doubleJumping = false;
      doubleJumpingHeight = planetR + gm.playerHeightOffset;
    }

    if (onSomething && jumping) {
      ducking = false;
      animator.SetBool("jumping", true);
      vSpeed = maxVSpeed;
      AudioManager.Instance().PlayJump();
      onGround = false;
    }

    if (onSomething) {
      animator.SetBool("jumping", false);
    }

    if (!onGround && !doubleJumping && jumping) {
      ducking = false;
      animator.SetBool("jumping", true);
      vSpeed = maxVSpeed;
      AudioManager.Instance().PlayJump();
      onGround = false;
      goDown = false;
      doubleJumping = true;
      doubleJumpingHeight = mobile.radius;
      onSomething = null;
    }

    if (onGround) {
      // // Se está no chão, deixe o personagem no chão
      animator.SetBool("jumping", false);
      vSpeed = 0;
      hSpeed = 0;
      mobile.radius = planetR + gm.playerHeightOffset - 1e-3f;
      goDown = false;

      if (jumping) {
        ducking = false;
        animator.SetBool("jumping", true);
        vSpeed = maxVSpeed;
        AudioManager.Instance().PlayJump();
        onGround = false;
        onSomething = null;
      }
      //                    << TODO arrumar esses valores de pulo aqui                 >>
    } else if ((goDown || (Mathf.Abs(mobile.radius) >= doubleJumpingHeight + gm.playerHeightOffset)) && !onSomething) {
      // Senão, aplique gravidade
      vSpeed -= gravity * Time.deltaTime;
      if (v < 0) {
        vSpeed -= gravity * 3f * Time.deltaTime;
      }
      goDown = true;
    }
    animator.SetFloat("vertical_speed", vSpeed);
    animator.SetBool("ducking", ducking);

    // Gravide aplica quando o botão solta
    if (releaseJump) {
      print(releaseJump);
      goDown = true;
    }

    float d = Dash();
    if (d != 0) {
      h = d;
    }

    trueHSpeed = h * maxHSpeed;
    if (hSpeed > 0) {
      hSpeed = Mathf.Max(hSpeed - hSpeed * airDrag * Time.deltaTime, 0);
    } else if (hSpeed < 0) {
      hSpeed = Mathf.Min(hSpeed - hSpeed * airDrag * Time.deltaTime, 0);
    }

    if (Mathf.Abs(h * maxHSpeed) > Mathf.Abs(hSpeed)) {
      trueHSpeed = h * maxHSpeed;
      hSpeed = 0;
    } else {
      trueHSpeed = hSpeed;
    }

    // Cálculo de posição vertical e horizontal
    mobile.Move(trueHSpeed, vSpeed);
    if (trueHSpeed != 0 || vSpeed != 0)
      HandleCollision();
    //}
    // Animar na horizontal se ele estiver se movendo
    animator.SetBool("horizontal_moving", Mathf.Abs(h) > 0);

    if (onSomething && onSomething.isActiveAndEnabled) {
      var bMob = onSomething.GetComponent<Mobile>();
      mobile.angle += onSomething.speed * Time.deltaTime / bMob.radius;
    }

    Shoot();
    Melee();
    Dash();
    //if (playerN == 1)
    //  Debug.LogFormat("Player{5}: p({0},{1}) v({4},{3}) {2}",mobile.angle, mobile.radius, onGround,vSpeed,trueHSpeed, playerN);
  }
}
