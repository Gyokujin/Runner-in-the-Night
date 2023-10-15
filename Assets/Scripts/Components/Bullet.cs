using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType
    {
        Player, Enemy
    }

    [SerializeField]
    private BulletType type;

    private Rigidbody2D rigid;
    private Animator animator;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void Shoot(Vector2 direction, float speed)
    {
        rigid.velocity = (direction * speed);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (type == BulletType.Player && collision.gameObject.layer == 11)
        {
            animator.SetTrigger("doHit");
        }
        else if (type == BulletType.Enemy && collision.gameObject.layer == 6)
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            player.Damage(false);
            animator.SetTrigger("doHit");
        }
    }

    void Hide()
    {
        Destroy(gameObject);
    }
}