using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Status")]
    [SerializeField]
    private float jumpFoece = 500f;

    [Header("Action")]
    private int jumpCount = 0;
    private bool onGround = true;
    private bool onDamage = false;
    private bool isDead = false;

    [Header("Components")]
    private Rigidbody2D rigid;
    private Animator animator;

    private PlayerAudio audio;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audio = GetComponent<PlayerAudio>();
    }

    void Update()
    {
        if (isDead)
            return;

        if (Input.GetMouseButtonDown(0) && jumpCount < 2)
        {
            jumpCount++;
            rigid.velocity = Vector2.zero;
            rigid.AddForce(Vector2.up * jumpFoece);
            audio.JumpSound();

        }
        else if (Input.GetMouseButtonUp(0) && rigid.velocity.y > 0)
        {
            rigid.velocity *= 0.5f;
        }

        animator.SetBool("onGround", onGround);
    }

    void Damage()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        
    }
}