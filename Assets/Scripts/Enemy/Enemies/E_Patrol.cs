using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Patrol : Enemy
{
    [Header("Move")]
    [SerializeField]
    private bool onGround = true; // 지상, 공중 몬스터를 구분
    private bool onMove = false;
    [SerializeField]
    private Transform[] landVec; // 0 : 왼쪽, 1 : 오른쪽
    [SerializeField]
    private float groundDis; // 발판 검사를 할 때 쏘는 레이의 길이
    [SerializeField]
    private float patrolTime;
    
    [Header("Detector")]
    private bool onDetect = false;

    [Header("Attack")]
    [SerializeField]
    private float patternDelay;
    [SerializeField]
    private float attackDelay;
    [SerializeField]
    private float shootSpeed;
    private bool onAttack;
    [SerializeField]
    private Transform emitter;
    [SerializeField]
    private GameObject bullet;

    void Start()
    {
        Think();
    }

    void Think()
    {
        if (onDetect)
        {
            animator.SetBool("onDetect", true);
        }
        else
        {
            animator.SetBool("onDetect", false);
            int pattern = Random.Range(0, 5);

            switch (pattern)
            {
                case 0:
                case 1:
                    Patrol(Vector2.left);
                    break;

                case 2:
                    Invoke("Think", patternDelay);
                    break;

                case 3:
                case 4:
                    Patrol(Vector2.right);
                    break;
            }
        }
    }

    void Patrol(Vector2 dir)
    {
        onMove = true;
        moveVec = dir;
        sprite.flipX = dir == Vector2.left ? false : true; // 왼쪽을 갈때는 flipX를 비활성화 오른쪽은 활성화한다.
        animator.SetBool("onMove", true);
        StartCoroutine("PatrolProcess");
    }

    IEnumerator PatrolProcess()
    {
        float time = patrolTime;
        rigid.velocity = moveVec * moveSpeed;

        while (time > 0 && GroundCheck())
        {
            time -= Time.deltaTime;
            yield return null;
        }

        onMove = false;
        animator.SetBool("onMove", false);
        rigid.velocity = Vector2.zero;

        new WaitForSeconds(patternDelay);
        Think();
    }

    bool GroundCheck()
    {
        Vector2 start = rigid.position;
        Vector2 dis = rigid.velocity.x < 0 ? landVec[0].localPosition : landVec[1].localPosition; // 좌 : landVec[0], 우 : landVec[1]
        start += dis; 

        Debug.DrawRay(start, Vector2.down * groundDis, Color.green);
        RaycastHit2D platCheck = Physics2D.Raycast(start, Vector2.down, groundDis, LayerMask.GetMask("Ground"));
        return platCheck;
    }

    public void Detect(Vector2 target)
    {
        if (onAttack)
            return;

        if (!onDetect)
        {
            onDetect = true;
            AudioManager.instance.PlaySystemSFX(AudioManager.SystemSFX.Detect);
        }

        rigid.velocity = Vector2.zero;
        Vector2 targetPos = target;
        sprite.flipX = false; // 왼쪽으로만 쏜다
        animator.SetBool("onMove", false);
        animator.SetBool("onDetect", true);
        StartCoroutine("Attack", targetPos);
    }

    IEnumerator Attack(Vector2 pos)
    {
        yield return new WaitForSeconds(attackDelay);

        if (onDie || transform.position.x < pos.x)
            yield break;

        animator.SetBool("onAttack", true);
        Vector2 dir = (pos - rigid.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 180f, Vector3.forward); // 블레스의 각도를 플레이어로 향하게 한다.

        GameObject spawnBullet = PoolManager.instance.Get(PoolManager.PoolType.Bullet, 1);
        spawnBullet.transform.position = emitter.position;
        spawnBullet.transform.rotation = rotation;
        spawnBullet.GetComponent<Bullet>().Shoot(dir, shootSpeed);

        switch (kind)
        {
            case EnemyKind.Blazer:
                AudioManager.instance.PlayEnemySFX(AudioManager.EnemySfx.BlazerAttack);
                break;
        }

        yield return new WaitForSeconds(patternDelay); // 텀을 주고 디텍터를 활성화
        animator.SetBool("onAttack", false);
        onAttack = false;
        detector.SetActive(true);
        Think();
    }
}