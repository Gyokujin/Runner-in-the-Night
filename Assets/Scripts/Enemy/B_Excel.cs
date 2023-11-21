using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Excel : MonoBehaviour
{
    public enum Phase
    {
        Phase1,
        Phase2,
        Phase3,
        Phase4,
    }

    private Phase phase; // 1 ~ 4페이즈까지 있으며 공격 패턴의 경우의 수를 구분한다.

    [Header("Status")]
    [SerializeField]
    private int maxHp;
    private int hp;
    [SerializeField]
    private float patternDelay;

    [Header("Attack")]
    [SerializeField]
    private Transform emitter;
    [SerializeField]
    private float generalSpeed;

    [Header("Component")]
    private Animator animator;
    private Rigidbody2D rigid;
    private BoxCollider2D collider;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();

        Init();
    }

    void Init()
    {
        phase = Phase.Phase1;
        hp = maxHp;
    }

    void Start()
    {
        Think();
    }

    void Think()
    {
        int patternIndex = 0;

        switch (phase)
        {
            case Phase.Phase1:
                patternIndex = 0;
                break;
            case Phase.Phase2:
                break;
            case Phase.Phase3:
                break;
            case Phase.Phase4:
                break;
        }

        switch (patternIndex)
        {
            case 0:
                GeneralShot();
                break;
        }
    }

    void GeneralShot()
    {
        GameObject spawnBullet = PoolManager.instance.Get(PoolManager.PoolType.Bullet, 2);
        spawnBullet.gameObject.SetActive(true);
        spawnBullet.transform.position = emitter.position;
        spawnBullet.GetComponent<Bullet>().Shoot(Vector2.left, generalSpeed);

        Invoke("Think", patternDelay);
    }
}