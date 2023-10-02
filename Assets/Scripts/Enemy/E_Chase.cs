using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Chase : Enemy
{
    private bool onDetect = false;
    [SerializeField]
    private float detectTime;
    [SerializeField]
    private float chaseSpeed;

    void Start()
    {
        rigid.velocity = moveVec;
    }

    public void Detect(GameObject target)
    {
        if (!onDetect)
        {
            onDetect = true;
            rigid.velocity = Vector2.zero;
            animator.SetBool("onDetect", true);
            StartCoroutine("Chase", target);
        }
    }

    IEnumerator Chase(GameObject vec)
    {
        yield return new WaitForSeconds(detectTime);
        moveVec = (vec.transform.position - transform.position).normalized;
        rigid.velocity = moveVec * chaseSpeed;
        animator.SetTrigger("doChase");
    }
}