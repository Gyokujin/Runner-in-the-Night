using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField]
    private float jumpFoece = 500f;
    private int jumpCount = 0;
    [SerializeField]
    private float rayDistance = 1.25f;
    [HideInInspector]
    public bool onGround = true;

    [Header("Status")]
    [SerializeField]
    private int maxLife = 3;
    private int life;
    
    [Header("Action")]
    private bool onDamage = false;
    private bool isDead = false;

    [Header("Components")]
    private BoxCollider2D collider;
    private Rigidbody2D rigid;
    private Animator animator;
    private PlayerAudio audio;

    void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audio = GetComponent<PlayerAudio>();
    }

    void Start()
    {
        life = maxLife;
    }

    void Update()
    {
        if (isDead)
            return;

        GroundCheck();
    }

    public void Jump()
    {
        if (!isDead && jumpCount < 2)
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
            onDamage = true;
            animator.SetTrigger("onHit");
        }
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
        if (collision.CompareTag("Enemy") || collision.CompareTag("Obstacle"))
        {
            Damage();
        }
    }
}