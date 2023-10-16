using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Chase : Enemy
{
    [SerializeField]
    private float detectTime;
    [SerializeField]
    private float chaseSpeed;

    void Start()
    {
        rigid.velocity = moveVec * moveSpeed;
    }

    public void Detect(GameObject target)
    {
        rigid.velocity = Vector2.zero;
        animator.SetBool("onDetect", true);
        StartCoroutine("Chase", target);
    }

    IEnumerator Chase(GameObject vec)
    {
        yield return new WaitForSeconds(detectTime);

        switch (kind)
        {
            case EnemyKind.Chaser:
                AudioManager.instance.PlayEnemySFX(AudioManager.EnemySfx.ChaserChase);
                break;
        }

        moveVec = (vec.transform.position - transform.position).normalized;
        rigid.velocity = moveVec * chaseSpeed;
        animator.SetTrigger("doChase");
    }
}