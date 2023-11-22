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

    [Header("Action")]
    [SerializeField]
    private B_Detect detector;

    [Header("Attack")]
    [SerializeField]
    private Transform emitter;
    [SerializeField]
    private float generalSpeed;
    [SerializeField]
    private float attackDelay;
    [SerializeField]
    private float attackDis; // 공격 유효거리

    // yield return time
    private WaitForSeconds patternWait;
    private WaitForSeconds attackWait;

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

        patternWait = new WaitForSeconds(patternDelay);
        attackWait = new WaitForSeconds(attackDelay);
    }

    void Start()
    {
        Invoke("Think", patternDelay);
    }

    void Think()
    {
        detector.gameObject.SetActive(true);
        Vector2 target = detector.targetPos;

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

        StartCoroutine("PatternChoice", patternIndex);
    }

    IEnumerator PatternChoice(int pattern)
    {
        switch (pattern)
        {
            case 0:
                yield return StartCoroutine("GeneralShot");
                break;
        }

        yield return patternWait;
        Think();
    }

    IEnumerator GeneralShot()
    {
        GameObject spawnBullet = PoolManager.instance.Get(PoolManager.PoolType.Bullet, 2);
        spawnBullet.gameObject.SetActive(true);
        spawnBullet.transform.position = emitter.position;
        spawnBullet.GetComponent<Bullet>().Shoot(Vector2.left, generalSpeed);

        yield return attackDelay;
    }
}