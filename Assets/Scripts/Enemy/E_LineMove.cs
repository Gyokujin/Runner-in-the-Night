using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_LineMove : Enemy
{
    void Start()
    {
        rigid.velocity = moveVec * moveSpeed;
    }
}