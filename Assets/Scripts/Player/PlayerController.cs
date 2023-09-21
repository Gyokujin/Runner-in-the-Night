using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Status")]
    private float landDis;
    private float jumpFoece;
    private float slideCool;
    private int maxLife;
    private float knockback;
    private float hitTime;
    private float invincibleTime;

    [Header("Action")]
    private int life;
    private int jumpCount = 0;
    private Vector2[] landVec = new Vector2[2];

    private bool onGround = true;
    private bool onSlide = false;
    private bool slideAble = true;
    private bool onDamage = false;
    private bool onInvincible = false;
    private bool isDead = false;

    [Header("Components")]
    private PlayerStatus status;
    private PlayerAudio audio;
    private SpriteRenderer sprite;
    private BoxCollider2D collider;
    private Rigidbody2D rigid;
    private Animator animator;

    void Awake()
    {
        status = GetComponent<PlayerStatus>();
        audio = GetComponent<PlayerAudio>();
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        StatusSetting();
        StartSetting();
    }

    void StatusSetting()
    {
        landDis = status.RayDistance;
        jumpFoece = status.JumpFoce;
        slideCool = status.SlideCoolTime;
        maxLife = status.MaxLife;
        knockback = status.KnockbackForce;
        hitTime = status.HitTime;
        invincibleTime = status.InvincibleTime;
    }

    void StartSetting()
    {
        life = maxLife;
        landVec[0] = transform.GetChild(0).position;
        landVec[1] = transform.GetChild(1).position;

        Debug.Log(landVec[0]);
        Debug.Log(landVec[1]);
    }

    void Update()
    {
        if (!isDead)
        {
            onGround = GroundCheck();
        }

        if (onGround)
        {
            Move();
        }
        else
        {
            Stop();
        }
    }

    void Move()
    {
        animator.SetBool("onMove", true);
        animator.SetBool("onGround", true);
    }

    void Stop()
    {
        animator.SetBool("onMove", false);
    }

    void Land()
    {
        jumpCount = 0;
    }

    bool GroundCheck()
    {
        Debug.DrawRay(landVec[0], Vector2.down * landDis, Color.green);
        Debug.DrawRay(landVec[1], Vector2.down * landDis, Color.green);
        RaycastHit2D platCheck1 = Physics2D.Raycast(landVec[0], Vector2.down, landDis, LayerMask.GetMask("Ground"));
        RaycastHit2D platCheck2 = Physics2D.Raycast(landVec[1], Vector2.down, landDis, LayerMask.GetMask("Ground"));

        return platCheck1 || platCheck2 ? true : false;
    }
    
    public void Jump() // 버튼으로 사용하기 때문에 public
    {
        if (!isDead && jumpCount < 2 && !onDamage)
        {
            jumpCount++;
            rigid.velocity = Vector2.zero;
            rigid.AddForce(Vector2.up * jumpFoece);
            onGround = false;

            audio.PlaySound("jump");

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                animator.Play("Jump", -1);
            }
            else
            {
                animator.SetTrigger("doJump");
                animator.SetBool("onGround", false);
            }
        }
    }

    /*
    public void StopJump()
    {
        if (!isDead && rigid.velocity.y > 0)
        {
            rigid.velocity *= 0.5f;
        }
    }

    public void Slide()
    {
        if (!isDead && onGround && !onDamage && slideAble)
        {
            StartCoroutine("SlideProcess");
        }
    }

    public void StopSlide()
    {
        StopCoroutine("SlideProcess");
        EndSlide();
    }

    void EndSlide()
    {
        animator.SetBool("onSlide", false);
        StartCoroutine("SlideCooldown");
        onSlide = false;
    }

    IEnumerator SlideProcess()
    {
        animator.SetBool("onSlide", true);
        onSlide = true;

        yield return new WaitForSeconds(slideTime);
        EndSlide();
    }

    IEnumerator SlideCooldown()
    {
        slideAble = false;
        float coolDown = slideCool;

        while (coolDown > 0)
        {
            coolDown -= Time.deltaTime;
            yield return null;
        }

        slideAble = true;
    }

    void Damage()
    {
        life--;
        rigid.velocity = Vector2.zero;
        audio.PlaySound("damage");

        if (life <= 0)
        {
            StartCoroutine("Die");
        }
        else
        {
            StartCoroutine("DamageProcess");
        }
    }

    IEnumerator DamageProcess()
    {
        onDamage = true;
        animator.SetBool("onMove", false);
        animator.SetTrigger("onHit");
        sprite.color = new Color(1, 1, 1, 0.7f);
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.left * knockbackForce);

        GameManager.instance.GamePause(); // 캐릭터외의 진행을 멈춘다. 스크롤링 일시정지

        yield return new WaitForSeconds(1f); // 1초 뒤에 바닥을 검사해서 있을 경우 그대로 진행 없을 경우 새로 진행한다.
        Invoke("StandUp", damageTime);
    }

    void StandUp()
    {
        animator.SetBool("onMove", true);
        onDamage = false;
        StartCoroutine("Invincible");
        GameManager.instance.GameRestart();
    }

    IEnumerator Invincible()
    {
        onInvincible = true;

        yield return new WaitForSeconds(invincibleTime);
        onInvincible = false;
        sprite.color = new Color(1, 1, 1, 1);
    }

    IEnumerator Die()
    {
        collider.enabled = false;
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * 600f);
        animator.SetTrigger("onDie");
        isDead = true;
        GameManager.instance.GameOver();

        yield return new WaitForSeconds(1f);
        rigid.gravityScale *= 2;

        yield return new WaitForSeconds(2f);
        rigid.simulated = false;
    }


    /*
    bool GroundCheck()
    {
        bool check = false;
        Debug.DrawRay(rigid.position, Vector2.down * rayDistance, Color.red);
        RaycastHit2D platCheck = Physics2D.Raycast(rigid.position, Vector2.down, rayDistance, LayerMask.GetMask("Ground"));

        if (platCheck.collider != null)
        {
            check = true;
        }

        return check;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (onSlide || onDamage || onInvincible)
            return;

        if (collision.CompareTag("Obstacle"))
        {
            Damage();
        }
    }
    */

}