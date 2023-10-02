using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Patrol : Enemy
{
    private bool onMove = false;
    private bool onDetect = false;
    [SerializeField]
    private bool onGround = true; // 지상, 공중 몬스터를 구분
    [SerializeField]
    private Vector2 landVec;

    [SerializeField]
    private float patrolTime;
    [SerializeField]
    private float patternDelay;
    [SerializeField]
    private float attackDelay;

    void Start()
    {
        Think();
    }

    void Think()
    {
        if (onDetect)
            return;

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

    void Patrol(Vector2 dir)
    {
        onMove = true;
        moveVec = dir;
        animator.SetBool("onMove", true);
        StartCoroutine("PatrolProcess");
    }

    IEnumerator PatrolProcess()
    {
        float time = patrolTime;
        rigid.velocity = moveVec * moveSpeed;

        while (time > 0 && GroundCheck(moveVec))
        {
            time -= Time.deltaTime;
            yield return null;
        }

        onMove = false;
        rigid.velocity = Vector2.zero;
    }

    bool GroundCheck(Vector2 dir)
    {
        bool check = false;

        return check;
    }

    public void Detect(GameObject target)
    {
        if (!onDetect)
        {
            onDetect = true;
            rigid.velocity = Vector2.zero;
            animator.SetBool("onMove", false);
            animator.SetBool("onDetect", true);
            StartCoroutine("Attack", target);
        }
    }

    IEnumerator Attack(GameObject vec)
    {

        yield return new WaitForSeconds(attackDelay);
        animator.SetTrigger("doAttack");
    }
}