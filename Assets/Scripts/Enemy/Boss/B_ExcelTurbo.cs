using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_ExcelTurbo : MonoBehaviour
{
    private BoxCollider2D collider;
    private Animator animator;

    void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    public void ControlEngine(bool engine)
    {
        animator.SetBool("onEngine", engine);
    }

    public void ControlBoost(bool boost)
    {
        collider.enabled = boost;
        animator.SetBool("onBoost", boost);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (!player.onDamage)
            {
                player.Hit();
            }
        }
    }
}