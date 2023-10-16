using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_LineMove : Enemy
{
    void Start()
    {
        Move();
    }

    void Move()
    {
        rigid.velocity = moveVec * moveSpeed;

        switch (kind)
        {
            case EnemyKind.Guardian:
                AudioManager.instance.PlayEnemySFX(AudioManager.EnemySfx.GuardianMove);
                break;
            case EnemyKind.Diver:
                AudioManager.instance.PlayEnemySFX(AudioManager.EnemySfx.DiverMove);
                break;
        }
    }
}