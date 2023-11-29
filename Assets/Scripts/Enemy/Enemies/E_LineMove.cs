using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_LineMove : Enemy
{
    public void Detect()
    {
        switch (kind)
        {
            case EnemyKind.Guardian:
                AudioManager.instance.PlayEnemySFX(AudioManager.EnemySfx.GuardianMove);
                break;
            case EnemyKind.Diver:
                AudioManager.instance.PlayEnemySFX(AudioManager.EnemySfx.DiverMove);
                break;
        }

        Move();
    }

    void Move()
    {
        rigid.velocity = moveVec * moveSpeed;
    }
}