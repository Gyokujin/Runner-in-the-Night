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
    }

    public Phase phase; // 1 ~ 3페이즈까지 있으며 공격 패턴의 경우의 수를 구분한다.

    [Header("Status")]
    [SerializeField]
    private int maxHp;
    private int hp;
    [SerializeField]
    private float patternDelay; // 패턴 실행전 딜레이

    [Header("Move")]
    [SerializeField]
    private float attackDisMin = 7.2f; // 공격전에 유지할 간격 최소값
    [SerializeField]
    private float attackDisMax = 8.8f; // 공격전에 유지할 간격 최대값
    [SerializeField]
    private float[] attackPosY; // 공격을 시작할 포지션Y
    [SerializeField]
    private float moveSpeed;

    [Header("Attack")]
    [SerializeField]
    private Transform emitter;
    [SerializeField]
    private float attackDelay; // 공격후 딜레이. 어떤 패턴이든 지연 시간을 발동한다.
    [SerializeField]
    private float generalShotSpeed;
    [SerializeField]
    private float impactShotSpeed;
    [SerializeField]
    private float shotDelay; // 기본 공격후 딜레이
    [SerializeField]
    private int comboShotCount;
    [SerializeField]
    private float comboShotDelay = 0.3f; // 트리플샷 공격 간의 딜레이
    [SerializeField]
    private float flameRushDis = 1f; // 플레임 러시로 플레이어에게 최대로 접근 하는 거리
    [SerializeField]
    private float flameRushSpeed = 2f;

    // yield return time
    private WaitForSeconds attackWait;
    private WaitForSeconds patternWait;
    private WaitForSeconds shotWait;
    private WaitForSeconds comboShotWait;

    [Header("Component")]
    private Animator animator;
    private Rigidbody2D rigid;
    private BoxCollider2D collider;
    private B_ExcelTurbo turbo;
    private GameObject player;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        turbo = GetComponentInChildren<B_ExcelTurbo>();
        player = GameObject.Find("Player");

        Init();
    }

    void Start()
    {
        StartCoroutine("PatternCycle");
    }

    void Init()
    {
        phase = Phase.Phase3;
        hp = maxHp;
        turbo.ControlEngine(true);

        attackWait = new WaitForSeconds(attackDelay);
        patternWait = new WaitForSeconds(patternDelay);
        shotWait = new WaitForSeconds(shotDelay);
        comboShotWait = new WaitForSeconds(comboShotDelay);
    }

    IEnumerator PatternCycle()
    {
        yield return patternWait;

        if (rigid.position.x - player.transform.position.x > attackDisMax) // 플레이어와 멀면 왼쪽 이동
        {
            StartCoroutine("Move", Vector2.left);
        }
        else if (rigid.position.x - player.transform.position.x < attackDisMin) // 플레이어와 가까우면 오른쪽 이동
        {
            StartCoroutine("Move", Vector2.right);
        }
        else // 적정 위치일 경우 공격 명령
        {
            int pattern = PatternChoice();

            switch (phase)
            {
                case Phase.Phase1: // 1페이즈 패턴 : GeneralShot(80%), ImpactShot(20%)
                    switch (pattern)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            StartCoroutine("GeneralShot");
                            break;
                        case 4:
                            StartCoroutine("ImpactShot");
                            break;
                    }
                    break;

                case Phase.Phase2: // 2페이즈 패턴 : ImpactShot(60%), ComboShot(40%)
                    switch (pattern)
                    {
                        case 0:
                        case 1:
                        case 2:
                            StartCoroutine("ImpactShot");
                            break;
                        case 3:
                        case 4:
                            StartCoroutine("ComboShot");
                            break;
                    }
                    break;
                
                case Phase.Phase3: // 3페이즈 패턴 : FlameRush
                    switch (pattern)
                    {
                        case 0:
                            StartCoroutine("FlameRush");
                            break;
                    }
                    break;
            }
        }
    }

    int PatternChoice()
    {
        int patternMin = 0;
        int patternMax = 0;

        switch (phase)
        {
            case Phase.Phase1:
            case Phase.Phase2:
                patternMin = 0;
                patternMax = 5;
                break;

            case Phase.Phase3:
                patternMin = 0;
                patternMax = 1;
                break;
        }
        
        int patternIndex = Random.Range(patternMin, patternMax);
        return patternIndex;
    }

    IEnumerator Move(Vector2 dir)
    {
        while (true)
        {
            rigid.velocity = dir * moveSpeed;
            float distance = rigid.position.x - player.transform.position.x;

            if (distance <= attackDisMax && distance >= attackDisMin)
            {
                break;
            }

            yield return null;
        }

        rigid.velocity = Vector2.zero;
        animator.SetBool("onDrive", false);
        StartCoroutine("PatternCycle");
    }

    IEnumerator GeneralShot()
    {
        int shotCount = Random.Range(1, 3); // 최대 2발까지 쏜다.

        for (int i = 0; i < shotCount; i++)
        {
            yield return shotWait;
            GameObject spawnBullet = PoolManager.instance.Get(PoolManager.PoolType.Bullet, 2);
            spawnBullet.transform.position = emitter.position;
            spawnBullet.GetComponent<Bullet>().Shoot(Vector2.left, generalShotSpeed);
            AudioManager.instance.PlayEnemySFX(AudioManager.EnemySfx.ExcelGeneralShot);
        }

        yield return attackWait;
        StartCoroutine("PatternCycle");
    }

    IEnumerator ImpactShot()
    {
        float movePosX = player.transform.position.x + attackDisMax; // 임팩트샷은 최대 사거리로 이동후 쏜다.
        
        while (rigid.position.x <= movePosX)
        {
            rigid.velocity = Vector2.right * moveSpeed;
            yield return null;
        }

        rigid.velocity = Vector2.zero;

        yield return shotWait;
        GameObject spawnBullet = PoolManager.instance.Get(PoolManager.PoolType.Bullet, 3);
        spawnBullet.transform.position = emitter.position;
        spawnBullet.GetComponent<Bullet>().Shoot(Vector2.left, impactShotSpeed);
        AudioManager.instance.PlayEnemySFX(AudioManager.EnemySfx.ExcelImpactShot);

        yield return attackWait;
        StartCoroutine("PatternCycle");
    }

    IEnumerator ComboShot()
    {
        int randomNum = -1;

        for (int i = 0; i < comboShotCount; i++) // 총 8번 사격
        {
            if (i == 0)
            {
                randomNum = 0; // 첫 발은 제자리에서 사격
            }
            else
            {
                randomNum = ComboShotPos(randomNum);
            }
            
            float posY = attackPosY[randomNum];
            
            if (rigid.position.y > posY) // 아래로 이동
            {
                while (true)
                {
                    rigid.velocity = Vector2.down * moveSpeed;

                    if (rigid.position.y <= posY)
                    {
                        break;
                    }

                    yield return null;
                }
            }
            else if (rigid.position.y < posY) // 위로 이동
            {
                while (true)
                {
                    rigid.velocity = Vector2.up * moveSpeed;

                    if (rigid.position.y >= posY)
                    {
                        break;
                    }

                    yield return null;
                }
            }

            rigid.velocity = Vector2.zero;
            GameObject spawnBullet = PoolManager.instance.Get(PoolManager.PoolType.Bullet, 2);
            spawnBullet.transform.position = emitter.position;
            spawnBullet.GetComponent<Bullet>().Shoot(Vector2.left, generalShotSpeed);
            AudioManager.instance.PlayEnemySFX(AudioManager.EnemySfx.ExcelGeneralShot);
            yield return comboShotWait;
        }

        if (rigid.position.y > attackPosY[0]) // 공격후 제자리로 이동
        {
            while (true)
            {
                rigid.velocity = Vector2.down * moveSpeed;

                if (rigid.position.y <= attackPosY[0])
                {
                    break;
                }

                yield return null;
            }
        }

        rigid.velocity = Vector2.zero;
        yield return attackWait;
        StartCoroutine("PatternCycle");
    }

    int ComboShotPos(int num)
    {
        int randomNum = 0;

        while (true)
        {
            randomNum = Random.Range(0, attackPosY.Length);

            if (randomNum == num)
                continue;
            else
                break;
        }

        return randomNum;
    }

    IEnumerator FlameRush()
    {
        animator.SetBool("onDrive", true);
        turbo.ControlEngine(false);
        turbo.ControlBoost(true);
        yield return attackWait;

        while (true)
        {
            rigid.velocity = Vector2.left * flameRushSpeed;
            float dis = rigid.position.x - player.transform.position.x;

            if (dis <= flameRushDis)
            {
                break;
            }

            yield return null;
        }

        turbo.ControlEngine(true);
        turbo.ControlBoost(false);
        yield return StartCoroutine("Move", Vector2.right); // Move를 실행함으로 PatternCycle을 대체한다.
    }
}