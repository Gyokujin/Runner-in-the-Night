using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
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
    private float currentLaunch;
    private bool onHit;

    private SpriteRenderer sprite;
    private Rigidbody2D rigid;
    private BoxCollider2D collider;
    private Animator animator;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    public void Shoot(Vector2 direction, float speed)
    {
        onHit = false;
        currentLaunch = launchTime; // 풀링으로 회수한 총알의 발사 시간을 초기화한다.
        rigid.velocity = (direction * speed);
        collider.enabled = true;
        animator.SetBool("onHit", false);
    }

    void Update()
    {
        if (!onHit)
        {
            currentLaunch -= Time.deltaTime;

            if (currentLaunch <= 0)
            {
                Hide();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((type == BulletType.Player && collision.GetComponent<Enemy>()) || (type == BulletType.Player && collision.GetComponent<B_Excel>()))
        {
            if (collision.GetComponent<Enemy>())
            {
                Enemy enemy = collision.GetComponent<Enemy>();

                if (!enemy.onDie)
                {
                    enemy.Damage();
                }
            }
            else if (collision.GetComponent<B_Excel>())
            {
                B_Excel excel = collision.GetComponent<B_Excel>();

                if (!excel.onDie)
                {
                    excel.Damage();
                }
            }

            onHit = true;
            animator.SetBool("onHit", true);
            collider.enabled = false;
            AudioManager.instance.PlaySystemSFX(AudioManager.SystemSFX.Hit);
        }
        else if (type == BulletType.Enemy && collision.GetComponent<PlayerController>())
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (!player.onDamage && player.gameObject.layer == 6)
            {
                player.Hit();
                onHit = true;
                animator.SetBool("onHit", true);
            }
        }
    }

    void Hide()
    {
        gameObject.transform.position = Vector2.zero;
        PoolManager.instance.Return(gameObject);
    }
}