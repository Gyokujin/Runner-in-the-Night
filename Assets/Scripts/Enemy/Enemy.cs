using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyKind
    {
        Guardian,
        Diver,
        Chaser,
        Wanderer,
        Blazer
    }

    public enum EnemyType
    {
        Fixed, // 고정형
        LineMove, // 일자 이동형
        Chase, // 추적형
        Patrol // 순찰형
    }

    public EnemyKind kind;
    public EnemyType type;

    [Header("Status")]
    [SerializeField]
    private int hp;
    
    [Header("Action")]
    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected Vector2 moveVec;
    protected bool onDie = false;

    [Header("Component")]
    protected SpriteRenderer sprite;
    protected Animator animator;
    protected Rigidbody2D rigid;
    protected BoxCollider2D collider;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    public void Damage()
    {
        hp--;

        if (hp <= 0)
        {
            Die(false);
        }
    }

    public void Die(bool outSide)
    {
        onDie = true;
        collider.enabled = false;

        if (outSide)
        {
            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine("DieProcess");
        }
    }

    IEnumerator DieProcess()
    {
        switch (kind)
        {
            case EnemyKind.Guardian:
                AudioManager.instance.PlayEnemySFX(AudioManager.EnemySfx.GuardianDie);
                break;
            case EnemyKind.Diver:
                AudioManager.instance.PlayEnemySFX(AudioManager.EnemySfx.DiverDie);
                break;
            case EnemyKind.Chaser:
                animator.SetTrigger("doDie");
                AudioManager.instance.PlayEnemySFX(AudioManager.EnemySfx.ChaserDie);
                break;
            case EnemyKind.Wanderer:
                AudioManager.instance.PlayEnemySFX(AudioManager.EnemySfx.WandererDie);
                break;
            case EnemyKind.Blazer:
                AudioManager.instance.PlayEnemySFX(AudioManager.EnemySfx.BlazerDie);
                break;
        }

        yield return null;
        animator.enabled = false;
        sprite.color = new Color(1, 1, 1, 0.65f);

        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}