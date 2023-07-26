using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Status")]
    [SerializeField]
    private float jumpFoece = 500f;

    // private int jumpCount; // 이후에 쓸지 검토
    private bool isGrounded = false; // 바닥에 닿았는지 나타냄
    private bool onDamage = false;

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
        
    }
}