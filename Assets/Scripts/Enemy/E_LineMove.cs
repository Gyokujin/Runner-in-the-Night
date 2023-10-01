using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class E_LineMove : Enemy
{
    [SerializeField]
    private Vector2 moveVec;

    void Start()
    {
        rigid.velocity = moveVec;
    }
}