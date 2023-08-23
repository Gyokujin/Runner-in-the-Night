using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField]
    private float rayDistance = 1.25f;
    [HideInInspector]
    public bool onGround = true;
    [SerializeField]
    private float jumpFoece = 500f;
    private int jumpCount = 0;
    [SerializeField]
    private float slideTime = 0.5f;
    [SerializeField]
    private float slideCool = 3f;
    private bool onSlide = false;
    private bool slideAble = true;

    [Header("Status")]
    [SerializeField]
    private int maxLife = 3;
    private int life;

    [Header("Action")]
    [SerializeField]
    private float knockbackForce;
    [SerializeField]
    private float damageTime = 1f;
    private bool onDamage = false;
    private float invincibleTime = 3f;
    private bool onInvincible = false;
    private bool isDead = false;

    [Header("Components")]
    private SpriteRenderer sprite;
    private BoxCollider2D collider;
    private Rigidbody2D rigid;
    private Animator animator;
    private PlayerAudio audio;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audio = GetComponent<PlayerAudio>();
    }

    void Start()
    {
        life = maxLife;
        animator.SetBool("onGround", true);
        animator.SetBool("onMove", true);
    }

    void Update()
    {
        if (isDead)
            return;

        GroundCheck();
    }

    public void Jump()
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
        onSlide = false;
    }

    IEnumerator SlideProcess()
    {
        animator.SetBool("onSlide", true);
        onSlide = true;

        yield return new WaitForSeconds(slideTime);
        StartCoroutine("SlideCooldown");
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
        GameManager.instance.GamePause(damageTime);
        sprite.color = new Color(1, 1, 1, 0.7f);
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.left * knockbackForce);

        yield return new WaitForSeconds(damageTime);
        animator.SetBool("onMove", true);
        onDamage = false;
        StartCoroutine("Invincible");
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

    void GroundCheck()
    {
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector2.down * rayDistance, Color.red);
            RaycastHit2D platCheck = Physics2D.Raycast(rigid.position, Vector2.down, rayDistance, LayerMask.GetMask("Ground"));

            if (platCheck.collider != null)
            {
                onGround = true;
                jumpCount = 0;
            }
        }

        animator.SetBool("onFall", rigid.velocity.y < 0 ? true : false);
        animator.SetBool("onGround", onGround);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (onSlide || onDamage || onInvincible)
            return;

        if (collision.CompareTag("Enemy") || collision.CompareTag("Obstacle"))
        {
            Damage();
        }
    }
}