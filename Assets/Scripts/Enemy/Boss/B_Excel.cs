using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class B_Excel : MonoBehaviour
{
    public enum Phase
    {
        Phase1,
        Phase2,
        Phase3,
    }

    public Phase phase; // 1 ~ 3페이즈까지 있으며 공격 패턴의 경우의 수를 구분한다.
    [SerializeField]
    private int[] phaseHp; // hp가 해당 phaseHp가 될때마다 페이즈를 넘어간다.

    [Header("Status")]
    [SerializeField]
    private int maxHp;
    private int hp;
    
    [HideInInspector]
    public bool onDie = false;
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
    private float shotDelay; // 기본 공격후 딜레이

    [Header("Attack_GeneralShot")]
    [SerializeField]
    private float generalShotSpeed;

    [Header("Attack_ImpactShot")]
    [SerializeField]
    private float impactShotSpeed;

    [Header("Attack_ComboShot")]
    [SerializeField]
    private int comboShotCount;
    [SerializeField]
    private float comboShotDelay = 0.3f; // 트리플샷 공격 간의 딜레이

    [Header("Attack_FlameSpear")]
    [SerializeField]
    private float flameSpearDis = 1f; // 플레임 스피어로 플레이어에게 최대로 접근 하는 거리
    [SerializeField]
    private float flameSpearSpeed = 2f;

    [Header("Attack_MachStrike")]
    [SerializeField]
    private float machStrikeStartDisX = 22.4f; // 마하 대시를 시전하기 위한 플레이어와의 X 간격
    [SerializeField]
    private int machStrikeCount = 5;
    private int machStrikeDirIndex = 4; // 공격 방향의 가짓수 (직선 이동 2, 대각선 이동 2)
    [SerializeField]
    private float machStrikeEndDisX = 8.6f; // 마하 대시를 끝내기 위한 플레이어와의 X 간격
    [SerializeField]
    private float machStrikeDirY = 0.2f; // 대각선 이동시의 Y 이동값.
    [SerializeField]
    private float machStrikeRotate; // 대각선 이동시의 rotation Z 값
    [SerializeField]
    private float machStrikeSpeed = 10f;
    [SerializeField]
    private float machStrikeStartDelay = 1f; // 마하대시 공격 간의 딜레이
    [SerializeField]
    private float machStrikeEndDelay = 0.3f; // 마하대시 공격 간의 딜레이
    [SerializeField]
    private Slider[] machStrikePaths;

    // yield return time
    private WaitForSeconds attackWait;
    private WaitForSeconds patternWait;
    private WaitForSeconds shotWait;
    private WaitForSeconds comboShotWait;
    private WaitForSeconds machDashStartWait;
    private WaitForSeconds machDashEndWait;

    [Header("Component")]
    private SpriteRenderer sprite;
    private Animator animator;
    private Rigidbody2D rigid;
    private BoxCollider2D collider;
    private B_ExcelTurbo turbo;
    private GameObject player;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        turbo = GetComponentInChildren<B_ExcelTurbo>();
        player = GameObject.Find("Player");
    }

    void Start()
    {
        Init();

        StartCoroutine("PatternCycle");
    }

    void Init()
    {
        phase = Phase.Phase1;
        hp = maxHp;
        turbo.ControlEngine(true);
        UIManager.instance.BossHPModify(true, maxHp, maxHp); // BossHP UI를 초기화한다

        attackWait = new WaitForSeconds(attackDelay);
        patternWait = new WaitForSeconds(patternDelay);
        shotWait = new WaitForSeconds(shotDelay);
        comboShotWait = new WaitForSeconds(comboShotDelay);
        machDashStartWait = new WaitForSeconds(machStrikeStartDelay);
        machDashEndWait = new WaitForSeconds(machStrikeEndDelay);
    }

    IEnumerator PatternCycle()
    {
        if (onDie)
            yield break;

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
                
                case Phase.Phase3: // 3페이즈 패턴 : ImpactShot(10%), FlameSpear(50%), MachStrike(40%)
                    switch (pattern)
                    {
                        case 0:
                            StartCoroutine("ImpactShot");
                            break;
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                            StartCoroutine("FlameSpear");
                            break;
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            StartCoroutine("MachStrike");
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
                patternMax = 10;
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

    IEnumerator MoveMaxDis() // 최대 사거리로 이동한다.
    {
        float movePosX = player.transform.position.x + attackDisMax; // 임팩트샷은 최대 사거리로 이동후 쏜다.

        while (rigid.position.x <= movePosX)
        {
            rigid.velocity = Vector2.right * moveSpeed;
            yield return null;
        }

        rigid.velocity = Vector2.zero;
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
        yield return StartCoroutine("MoveMaxDis"); // 최대 사거리로 이동
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
        yield return StartCoroutine("MoveMaxDis"); // 최대 사거리로 이동
        int randomNum = -1;

        for (int i = 0; i < comboShotCount; i++) // comboShotCount 만큼 반복한다.
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

    IEnumerator FlameSpear()
    {
        yield return StartCoroutine("MoveMaxDis"); // 최대 사거리로 이동
        animator.SetBool("onDrive", true);
        turbo.ControlEngine(false);
        turbo.BoostStart();

        while (true)
        {
            rigid.velocity = Vector2.left * flameSpearSpeed;
            float dis = rigid.position.x - player.transform.position.x;

            if (dis <= flameSpearDis || rigid.position.x < player.transform.position.x) // 너무 가깝거나 플레이어보다 왼쪽으로 갈 경우 종료
            {
                break;
            }

            yield return null;
        }

        turbo.BoostEnd();
        turbo.ControlEngine(true);
        rigid.velocity = Vector2.zero;
        yield return attackWait;
        yield return StartCoroutine("Move", Vector2.right); // Move를 실행함으로 PatternCycle을 대체한다.
    }

    IEnumerator MachStrike()
    {
        animator.SetBool("onDetect", true);
        float targetPosX = player.transform.position.x + machStrikeStartDisX;

        while (rigid.position.x <= targetPosX) // 기술을 시전하기 위해 카메라 밖으로 이동
        {
            rigid.velocity = Vector2.right * moveSpeed;
            yield return null;
        }

        rigid.velocity = Vector2.zero;
        
        yield return attackWait; // 잠깐 딜레이를 준다.
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        animator.SetBool("onDetect", false);
        animator.SetBool("onDrive", true);
        Vector2 startPos = transform.position; // 공격을 시전한 최초 위치

        for (int i = 0; i < machStrikeCount; i++) // machStrikeCount 수만큼 반복한다.
        {
            int attackIndex = Random.Range(0, machStrikeDirIndex);
            float endXPos = player.transform.position.x - machStrikeEndDisX;
            Vector2 attackPos = Vector2.zero;
            Vector2 dashDir = Vector2.zero; // 공격 방향의 벡터
            Quaternion attackRotation = Quaternion.identity;

            switch (attackIndex)
            {
                case 0: // 낮은 직선 이동
                    attackPos = new Vector2(startPos.x, attackPosY[0]);
                    dashDir = Vector2.left;
                    break;
                case 1: // 높은 직선 이동
                    attackPos = new Vector2(startPos.x, attackPosY[1]);
                    dashDir = Vector2.left;
                    break;
                case 2: // 높은 대각선 이동
                    attackPos = new Vector2(startPos.x, attackPosY[0]);
                    dashDir = new Vector2(-1, machStrikeDirY);
                    attackRotation = Quaternion.Euler(new Vector3(0, 0, -machStrikeRotate));
                    break;
                case 3: // 낮은 대각선 이동
                    attackPos = new Vector2(startPos.x, attackPosY[1]);
                    dashDir = new Vector2(-1, -machStrikeDirY);
                    attackRotation = Quaternion.Euler(new Vector3(0, 0, machStrikeRotate));
                    break;
            }

            machStrikePaths[attackIndex].gameObject.SetActive(true); // 공격 경로 UI 활성화
            transform.position = attackPos;
            transform.rotation = attackRotation;
            
            yield return machDashStartWait; // UI를 보고 예상할 시간을 준다.
            machStrikePaths[attackIndex].gameObject.SetActive(false); // 공격 경로 UI 비활성화

            AudioManager.instance.PlayEnemySFX(AudioManager.EnemySfx.ExcelMachStrike);

            while (rigid.position.x >= endXPos)
            {
                rigid.velocity = dashDir * machStrikeSpeed;
                yield return null;
            }

            rigid.velocity = Vector2.zero;
            yield return machDashEndWait; // 다음 공격 간의 텀을 둔다
        }

        transform.position = startPos; // 다시 위치와 각도 크기를 초기화한다.
        transform.rotation = Quaternion.identity;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

        yield return attackWait;
        StartCoroutine("PatternCycle");
    }

    public void Damage()
    {
        hp--;

        if (hp <= 0)
        {
            UIManager.instance.BossHPModify(false);
            Die();
        }
        else
        {
            UIManager.instance.BossHPModify(true, maxHp, hp);

            if (hp >= phaseHp[0])
            {
                phase = Phase.Phase1;
            }
            else if (hp >= phaseHp[1])
            {
                phase = Phase.Phase2;
            }
            else
            {
                phase = Phase.Phase3;
            }
        }
    }

    void Die()
    {
        StopAllCoroutines();
        onDie = true;
        BossStageManager.instance.BossDefeat();
    }
}