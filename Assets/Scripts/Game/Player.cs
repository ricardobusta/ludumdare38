using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents a playable character. Control it's movements and actions.
/// </summary>
public class Player : MonoBehaviour {

  [Header("Parameters")]
  public int playerN;
  public Color bulletColor;
  int maxPlayerLives;
  public AudioClip hitSound;

  [Header("Status")]
  public int playerLives = 3;
  public int ammoLeft = 5;
  public bool mounting = false;
  public bool onGround = false;
  public Bullet onBullet = null;
  public bool goDown = true;
  bool ducking = false;
  public bool doubleJumping = false;
  float doubleJumpingHeight = 0;
  // Movement information
  public float maxHSpeed = 180;
  public float maxVSpeed = 3;
  private float gravity = 8f;
  private float airDrag = 3f;

  // Player final speed
  public float vSpeed = 0;
  public float hSpeed = 0;
  private float trueHSpeed = 0;

  //Variáveis de controle da invulnerabilidade;
  public float invulnerabilityDuration = 1;
  float invulnerabilityCurrent = 0;
  bool firstTimeOnGround = false;
  bool dead = false;

  [Header("Control")]
  public float horizontalInput = 0;
  public float verticalInput = 0;
  public bool meleeInput = false;
  public float meleeCD = 1;
  float currentMeleeCD = 0;
  public bool fireInput = false;
  public float fireCD = 1;
  float currentFireCD = 0;
  //Dash Control
  public bool dashInput = false;
  public float dashCD = 1;
  float currentDashCD = 0;
  public float dashDuration = 0.5f;
  float currentDashDuration = -1;
  float dashDirection = 0;
  public float dashSpeedMultiplier = 4;
  //Jump Control
  bool jumpInput = false;
  bool jumpReleaseInput = false;

  [Header("References")]
  public GameManager gm;
  new Collider2D collider;
  ContactFilter2D playerFilter = new ContactFilter2D();
  Collider2D[] obstacles = new Collider2D[10];
  RaycastHit2D[] rayResults = new RaycastHit2D[2];
  public BulletCounter bulletCounter;
  public FaceManager faceManager;
  public Image flowerCDindicator;
  Animator animator;
  Mobile mobile;
  public GameObject shootPoint;

  private void Awake() {
    mobile = GetComponent<Mobile>();
    animator = GetComponent<Animator>();
    collider = GetComponent<Collider2D>();
  }

  /// <summary>
  /// 
  /// </summary>
  private void Start() {
    playerLives = Options.playerLives;
    maxPlayerLives = playerLives;
    ammoLeft = Options.playerBullets;
    bulletCounter.SetBulletCount(ammoLeft);
    playerFilter.SetLayerMask(LayerMask.GetMask("Player"));
    playerFilter.useLayerMask = true;
    animator.SetInteger("player_health", playerLives);
  }

  /// <summary>
  /// 
  /// </summary>
  void HandleCollision() {
    float r = mobile.radius;
    if (collider.GetContacts(playerFilter, obstacles) > 0) {
      ColliderDistance2D d = collider.Distance(obstacles[0]);
      //Debug.DrawLine(d.pointA, d.pointB);
      var v = d.pointA - d.pointB;
      var p2 = obstacles[0].GetComponent<Player>();
      var p2m = obstacles[0].GetComponent<Mobile>();
      var n = transform.up;
      //onGround = false;
      //goDown = true;
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
      if (onGround) {
        mobile.radius = r;
      }
    }
  }

  /// <summary>
  /// 
  /// </summary>
  //Todo: Maybe transformar isso em property?
  /// <returns>Returns true if player is invunerable, false if it is not.</returns>  
  public bool Invulnerable() {
    return invulnerabilityCurrent > 0;
  }

  /// <summary>
  /// Initiate the firing animation. Don't actually shoot the bullet. That's what the animation do.
  /// </summary>
  void FireAction() {
    if (currentFireCD > 0) {
      // Se o cooldown de tiro é positivo, decremente
      currentFireCD -= Time.deltaTime;
    } else if (ammoLeft > 0 && fireInput) {
      // Senão, deixe o jogador atirar
      animator.SetTrigger("fire");
      print("shoot!");
      currentFireCD = fireCD;
    }
  }

  /// <summary>
  /// Script that executes when the animation hits the shooting frame, which actually releases the bullet and plays the fire sound.
  /// </summary>
  void FireBullet() {
    Bullet b = GameManager.Instance.GetFreeBullet();
    if (b != null) {
      ammoLeft--;
      bulletCounter.SetBulletCount(ammoLeft);
      b.Activate(mobile, shootPoint);
      if (onBullet && onBullet.isActiveAndEnabled) {
        var bMob = onBullet.GetComponent<Mobile>();
        float rate = 0.33f * mobile.radius / bMob.radius;
        b.speed = Mathf.Abs(b.initialSpeed) * mobile.direction + onBullet.speed * rate;
        //onSomething.speed -= Mathf.Abs(speed) * mobile.direction;
        //bMob.refresh();
      }
      b.SetColor(bulletColor);
      b.gameObject.SetActive(true);
      AudioManager.Instance().PlayFire();
    }
  }

  /// <summary>
  /// Verify if the player has attacked with melee strike.
  /// </summary>
  void Melee() {
    if (currentMeleeCD > 0) {
      // Se o cooldown de tiro é positivo, decremente
      currentMeleeCD -= Time.deltaTime;
    } else if (meleeInput) {
      currentMeleeCD = meleeCD;
      animator.SetTrigger("attack");
      AudioManager.Instance().PlayAttack();
    }
    if (flowerCDindicator != null) {
      float f = Mathf.Clamp01(currentMeleeCD / meleeCD);
      flowerCDindicator.fillAmount = f;
    }
  }

  /// <summary>
  /// Verifies if the player is dashing and executes the move..
  /// </summary>
  /// <returns></returns>
  float Dash() {
    // Durante o dash, o jogador se comporta de um jeito diferente
    if (currentDashDuration < 0) {
      if (currentDashCD > 0) {
        // Se o cooldown de dash é positivo, decremente
        currentDashCD -= Time.deltaTime;
      } else if (dashInput) {
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

  /// <summary>
  /// Enables the character, and every asset that's connected to it (like the character face and character bullet count.)
  /// </summary>
  /// <param name="active"></param>
  public void SetActive(bool active) {
    faceManager.gameObject.SetActive(active);
    bulletCounter.gameObject.SetActive(active);
    gameObject.SetActive(active);
  }

  /// <summary>
  /// Makes the user take damage.
  /// </summary>
  /// <param name="i">amount of damage taken</param>
  public void TakeDamage(int i = 1) {
    if (invulnerabilityCurrent > 0) {
      return;
    }
    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) {
      return;
    }
    invulnerabilityCurrent = invulnerabilityDuration;
    GameManager.Instance.StartScreenShake();
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

  /// <summary>
  /// 
  /// </summary>
  /// <param name="angle"></param>
  public void Position(float angle) {
    mobile.radius = gm.planetRadius + gm.playerHeightOffset;
    mobile.angle = angle - 90;
    onGround = true;
    mobile.refresh();
  }

  private void HandleControls() {
    fireInput = Input.GetButton("P" + playerN + "_Fire");
    dashInput = Input.GetButtonDown("P" + playerN + "_Dash");
    horizontalInput = Input.GetAxisRaw("P" + playerN + "_Horizontal");
    verticalInput = Input.GetAxisRaw("P" + playerN + "_Vertical");
    meleeInput = Input.GetButton("P" + playerN + "_Melee");
    jumpInput = Input.GetButtonDown("P" + playerN + "_Jump") || (Input.GetButtonDown("P" + playerN + "_Vertical") && verticalInput > 0);
    jumpReleaseInput = Input.GetButtonUp("P" + playerN + "_Jump") || (Input.GetButtonUp("P" + playerN + "_Vertical"));
  }

  /// <summary>
  /// 
  /// </summary>
  public void HandleLogic() {
    if (dead) {
      gameObject.SetActive(false);
      return;
    }

    if (gm.pause || gm.gameOver) {
      animator.speed = 0;
      return;
    }
    animator.speed = 1;

    if (invulnerabilityCurrent > 0) {
      invulnerabilityCurrent -= Time.deltaTime;
    }

    HandleControls();

    animator.SetBool("mounting", mounting);

    animator.SetBool("invulnerable", invulnerabilityCurrent > 0);

    if (mounting) {
      if (horizontalInput != 0 && horizontalInput != mobile.direction) {
        mobile.direction = horizontalInput;
      }
      horizontalInput = mobile.direction;
    }

    float planetR = gm.planetRadius;

    // Está no chão se o raio for menor igual que o planeta + altura do jogador
    bool wasOnGround = onGround;
    onGround = (Mathf.Abs(mobile.radius) <= planetR + gm.playerHeightOffset);

    if (!wasOnGround && onGround) {
      AudioManager.Instance().PlayLanding();
    }

    ducking = false;
    if ((onGround || onBullet) && horizontalInput == 0 && verticalInput < 0) {
      ducking = true;
    }

    if (onGround || onBullet) {
      doubleJumping = false;
      doubleJumpingHeight = planetR + gm.playerHeightOffset;
    }

    if (onBullet && jumpInput) {
      ducking = false;
      animator.SetBool("jumping", true);
      vSpeed = maxVSpeed;
      AudioManager.Instance().PlayJump();
      onGround = false;
    }

    if (onBullet) {
      animator.SetBool("jumping", false);
    }

    if (!(onGround || onBullet) && !doubleJumping && jumpInput) {
      ducking = false;
      animator.SetBool("jumping", true);
      vSpeed = maxVSpeed;
      AudioManager.Instance().PlayJump();
      onGround = false;
      goDown = false;
      doubleJumping = true;
      doubleJumpingHeight = mobile.radius;
      onBullet = null;
    }

    if (onGround) {
      // // Se está no chão, deixe o personagem no chão
      animator.SetBool("jumping", false);
      vSpeed = 0;
      hSpeed = 0;
      mobile.radius = planetR + gm.playerHeightOffset - 1e-3f;
      goDown = false;

      if (jumpInput) {
        ducking = false;
        animator.SetBool("jumping", true);
        vSpeed = maxVSpeed;
        AudioManager.Instance().PlayJump();
        onGround = false;
        onBullet = null;
      }
      //                    << TODO arrumar esses valores de pulo aqui                 >>
    } else if ((goDown || (Mathf.Abs(mobile.radius) >= doubleJumpingHeight + gm.playerHeightOffset)) && !onBullet) {
      // Senão, aplique gravidade
      vSpeed -= gravity * Time.deltaTime;
      if (verticalInput < 0) {
        vSpeed -= gravity * 3f * Time.deltaTime;
      }
      goDown = true;
    }
    animator.SetFloat("vertical_speed", vSpeed);
    animator.SetBool("ducking", ducking);

    // Gravide aplica quando o botão solta
    if (jumpReleaseInput) {
      print(jumpReleaseInput);
      goDown = true;
    }

    float d = Dash();
    if (d != 0) {
      horizontalInput = d;
    }

    trueHSpeed = horizontalInput * maxHSpeed;
    if (hSpeed > 0) {
      hSpeed = Mathf.Max(hSpeed - hSpeed * airDrag * Time.deltaTime, 0);
    } else if (hSpeed < 0) {
      hSpeed = Mathf.Min(hSpeed - hSpeed * airDrag * Time.deltaTime, 0);
    }

    if (Mathf.Abs(horizontalInput * maxHSpeed) > Mathf.Abs(hSpeed)) {
      trueHSpeed = horizontalInput * maxHSpeed;
      hSpeed = 0;
    } else {
      trueHSpeed = hSpeed;
    }

    // Animar na horizontal se ele estiver se movendo
    animator.SetBool("horizontal_moving", Mathf.Abs(horizontalInput) > 0);

    // Cálculo de posição vertical e horizontal
    mobile.Move(trueHSpeed, vSpeed);

    if (onBullet && onBullet.isActiveAndEnabled) {
      var bMob = onBullet.GetComponent<Mobile>();
      mobile.angle += onBullet.speed * Time.deltaTime / bMob.radius;
    }

    FireAction();
    Melee();
    Dash();
  }

  public void HandlePhysics() {
    if (dead) {
      gameObject.SetActive(false);
      return;
    }
    if (gm.pause || gm.gameOver) { return; }

    float planetR = gm.planetRadius;

    if (onBullet && onBullet.isActiveAndEnabled) {
      var nCast = collider.Cast(-transform.up, rayResults, planetR);
      if (nCast > 0) {
        if (!rayResults[0].transform.name.Contains("Bullet")) {
          onBullet = null;
        } else if (rayResults[0].distance > 0.35) {
          onBullet = null;
        }
      } else {
        onBullet = null;
      }
    } else {
      onBullet = null;
    }


    if (trueHSpeed != 0 || vSpeed != 0) {
      HandleCollision();
    }
  }
}
