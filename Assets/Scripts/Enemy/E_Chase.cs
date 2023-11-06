using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Chase : Enemy
{
    [SerializeField]
    private float detectTime;
    [SerializeField]
    private float detectDelay;
    [SerializeField]
    private float chaseSpeed;

    void Start()
    {
        rigid.velocity = moveVec * moveSpeed;
    }


    public void Detect(Vector2 target)
    {
        rigid.velocity = Vector2.zero;
        animator.SetTrigger("doDetect");
        AudioManager.instance.PlaySystemSFX(AudioManager.SystemSFX.Detect);
        StartCoroutine("ReadyChase", target.y);
    }

    IEnumerator ReadyChase(float movePosY)
    {
        yield return new WaitForSeconds(detectDelay);
        transform.position = new Vector3(transform.position.x, movePosY);
        animator.SetTrigger("doReady");

        yield return new WaitForSeconds(detectDelay);
        StartCoroutine("Chase");
    }

    IEnumerator Chase()
    {
        yield return new WaitForSeconds(detectTime);

        switch (kind)
        {
            case EnemyKind.Chaser:
                AudioManager.instance.PlayEnemySFX(AudioManager.EnemySfx.ChaserChase);
                break;
        }

        rigid.velocity = moveVec * chaseSpeed;
        animator.SetTrigger("doChase");
    }
}