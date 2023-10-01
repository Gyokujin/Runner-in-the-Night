using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Status")]
    [SerializeField]
    private int maxHp;
    private int hp;

    [Header("Component")]
    protected Rigidbody2D rigid;
    protected BoxCollider2D collider;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        hp = maxHp;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            Debug.Log("Enemy ÇÇ°Ý");
        }

        if (collision.gameObject.layer == 10) // Deadzone
        {
            Debug.Log("Deadzone");
            gameObject.SetActive(false);
        }
    }
}