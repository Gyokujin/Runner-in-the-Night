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
    [SerializeField]
    private float launchTime;

    private Rigidbody2D rigid;
    private BoxCollider2D collider;
    private Animator animator;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        collider.enabled = true;
    }

    public void Shoot(Vector2 direction, float speed)
    {
        rigid.velocity = (direction * speed);
        Invoke("Hide", launchTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (type == BulletType.Player && collision.gameObject.layer == 11)
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (!enemy.onDie)
            {
                enemy.Damage();
                animator.SetTrigger("doHit");
                collider.enabled = false;
                AudioManager.instance.PlaySystemSFX(AudioManager.SystemSFX.Hit);
                Invoke("Hide", launchTime);
            }
        }
        else if (type == BulletType.Enemy && collision.gameObject.layer == 6)
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (!player.onDamage)
            {
                player.Damage(false);
                animator.SetTrigger("doHit");
                Invoke("Hide", launchTime);
            }
        }
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}