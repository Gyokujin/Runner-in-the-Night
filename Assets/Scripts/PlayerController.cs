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
    private bool onGround = true;

    [Header("Status")]
    private int maxLife = 3;
    private int life;
    
    [Header("Action")]
    private bool onDamage = false;
    private bool isDead = false;

    [Header("Components")]
    private Rigidbody2D rigid;
    private Animator animator;
    private PlayerAudio audio;

    void Awake()
    {
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
        animator.SetTrigger("onHit");
        audio.PlaySound("damage");
        rigid.velocity = Vector2.zero;
        onDamage = true;
        life--;

        if (life <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
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