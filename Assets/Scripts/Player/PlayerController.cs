using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Status
    private float landDis;
    private float jumpTime;
    private float jumpCool;
    private float jumpFoece;
    private float slideTime;
    private float slideCool;
    private int maxLife;
    private float knockback;
    private float bounce;
    private float hitTime;
    private float invincibleTime;

    // Action
    private int jumpCount = 0;
    private bool onJumping = false;
    private bool jumpAble = true;
    private Transform[] landVec = new Transform[2];
    private bool onGround = false;
    private bool onFall = false;
    private bool onSlide = false;
    private int playerLayer = 6; // Player 레이어
    private int invincibleLayer = 7; // Invincible 레이어
    private float respawnPosY = 5f; // 낙사로 인한 리스폰시에 이동시킬 Y 좌표

    // Hit
    private int life;
    private bool onDamage = false;
    private bool isDead = false;

    // yield return time
    private WaitForSeconds slideWait;
    private WaitForSeconds hitWait;

    // Component
    private PlayerStatus status;
    private PlayerAudio audio;
    private SpriteRenderer sprite;
    private Rigidbody2D rigid;
    private BoxCollider2D collider;
    private Animator animator;

    void Awake()
    {
        status = GetComponent<PlayerStatus>();
        audio = GetComponent<PlayerAudio>();
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        StatusSetting();
        StartSetting();
        WaitTimeSetting();
    }

    void StatusSetting()
    {
        landDis = status.RayDistance;
        jumpTime = status.JumpTime;
        jumpCool = status.JumpCoolTime;
        jumpFoece = status.JumpFoce;
        slideTime = status.SlideTime;
        slideCool = status.SlideCoolTime;
        maxLife = status.MaxLife;
        knockback = status.KnockbackForce;
        bounce = status.BounceForce;
        hitTime = status.HitTime;
        invincibleTime = status.InvincibleTime;
    }

    void StartSetting()
    {
        life = maxLife;
        landVec[0] = transform.GetChild(0);
        landVec[1] = transform.GetChild(1);
        gameObject.layer = playerLayer;

        Move(true);
    }

    void WaitTimeSetting()
    {
        slideWait = new WaitForSeconds(slideTime);
        hitWait = new WaitForSeconds(hitTime);
    }

    void Update()
    {
        if (isDead)
            return;

        if (!onDamage && !onGround && !onJumping && !onSlide && GroundCheck(landDis))
        {
            Land();
        }

        Fall(rigid.velocity.y < -0.1f ? true : false); // 0일 경우는 작은 반동에도 애니메이션 오류가 나타난다.
    }

    void Move(bool move)
    {
        animator.SetBool("onMove", move);
    }

    void Land()
    {
        onGround = true;
        jumpCount = 0;
        animator.SetBool("onGround", true);
        animator.SetBool("onFall", false);
    }

    bool GroundCheck(float distance)
    {
        Vector2 landPos1 = new Vector2(transform.position.x + landVec[0].localPosition.x, transform.position.y + landVec[0].localPosition.y);
        Vector2 landPos2 = new Vector2(transform.position.x + landVec[1].localPosition.x, transform.position.y + landVec[1].localPosition.y);
        Debug.DrawRay(landPos1, Vector2.down * distance, Color.green);
        Debug.DrawRay(landPos2, Vector2.down * distance, Color.green);
        RaycastHit2D platCheck1 = Physics2D.Raycast(landPos1, Vector2.down, distance, LayerMask.GetMask("Ground"));
        RaycastHit2D platCheck2 = Physics2D.Raycast(landPos2, Vector2.down, distance, LayerMask.GetMask("Ground"));

        return platCheck1 || platCheck2 ? true : false;
    }
    
    public void Jump() // 버튼으로 사용하기 때문에 public
    {
        if (!isDead && jumpCount < 2 && jumpAble && !onDamage)
        {
            if (onSlide)
            {
                SlideCancel();
            }

            onGround = false;
            onJumping = true;
            onFall = false;
            jumpAble = false;
            jumpCount++;
            animator.SetBool("onGround", false);

            rigid.velocity = Vector2.zero;
            rigid.AddForce(Vector2.up * jumpFoece);
            animator.SetTrigger("doJump");
            audio.PlaySound("jump");
            Invoke("JumpCool", jumpCool);
            Invoke("JumpTime", jumpTime); // 연속적인 Jump와 착지 오류를 막기위해 onJump에 딜레이를 준다.
        }
    }

    public void OnJump(bool click) // 버튼으로 사용하기 때문에 public
    {
        rigid.gravityScale = click ? 0.75f : 1.1f;
    }

    void JumpCool()
    {
        jumpAble = true;
    }

    void JumpTime()
    {
        onJumping = false;
    }

    void Fall(bool fall)
    {
        onFall = fall;
        animator.SetBool("onFall", fall);
    }

    public void Slide()
    {
        if (!isDead && GroundCheck(landDis) && !onDamage && !onSlide)
        {
            StartCoroutine("SlideProcess");
        }
    }

    IEnumerator SlideProcess()
    {
        onSlide = true;
        animator.SetBool("onSlide", true);
        audio.PlaySound("slide");
        StartCoroutine("Invincible", slideTime);
        UIManager.instance.ButtonCooldown("slide", slideCool); // 슬라이드의 쿨타임 계산

        yield return slideWait;
        onSlide = false;
        animator.SetBool("onSlide", false);
    }

    void SlideCancel()
    {
        StopCoroutine("SlideProcess"); // 슬라이드중 점프로 내부의 bool을 초기화한다.
        onSlide = false;
        animator.SetBool("onSlide", false);
        gameObject.layer = playerLayer;
    }

    public void Damage(bool outSide) // AttackBox에서 Damage() 호출
    {
        GameManager.instance.GamePause(); // 캐릭터외의 진행을 멈춘다. 스크롤링 일시정지
        life--;
        UIManager.instance.DamageUI(life);
        onDamage = true;
        rigid.velocity = Vector2.zero;
        Move(false);

        if (life <= 0)
        {
            StartCoroutine("Die");
        }
        else
        {
            StopCoroutine("DamageProcess");
            StartCoroutine("DamageProcess", outSide);
        }
    }

    IEnumerator DamageProcess(bool onOut)
    {
        audio.PlaySound("damage");
        sprite.color = new Color(1, 1, 1, 0.7f);

        if (onOut) // 낙사(DeadZone)로 인한 재등장
        {
            yield return StartCoroutine("Respawn");
        }
        else // 일반적인 피격
        {
            yield return StartCoroutine("HitProcess");
            GameManager.instance.GameResume(); // 피격은 바로 게임을 진행 / 낙사는 타이밍을 따로 정해서 진행한다.
        }
        
        onDamage = false;
        Move(true);

        yield return StartCoroutine("Invincible", invincibleTime);
        sprite.color = new Color(1, 1, 1, 1);
    }

    IEnumerator Respawn()
    {
        rigid.simulated = false; // 리스폰 처리 중에는 rigidbody를 비활성화한다.
        sprite.enabled = false;
        transform.position = new Vector2(transform.position.x, respawnPosY);
        GameManager.instance.GameResume();

        while (!GroundCheck(6f))
        {
            yield return null;
        }

        rigid.simulated = true;
        sprite.enabled = true;
        audio.PlaySound("respawn");
        UIManager.instance.RespawnFX(transform.position);
    }

    IEnumerator HitProcess()
    {
        animator.SetTrigger("onHit");
        rigid.AddForce(Vector2.left * knockback);

        yield return hitWait; // 1초 뒤에 바닥을 검사해서 있을 경우 그대로 진행 없을 경우 새로 진행한다.
    }

    IEnumerator Invincible(float time)
    {
        gameObject.layer = invincibleLayer;

        yield return new WaitForSeconds(time);
        gameObject.layer = playerLayer;
    }

    IEnumerator Die()
    {
        isDead = true;
        collider.enabled = false;
        animator.SetTrigger("doDie");
        audio.PlaySound("die");
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * bounce);
        GameManager.instance.GameOver();

        yield return new WaitForSeconds(1f);
        rigid.gravityScale *= 2;

        yield return new WaitForSeconds(2f);
        rigid.simulated = false;
    }
}