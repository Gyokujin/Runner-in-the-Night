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

    public void BoostStart()
    {
        animator.SetTrigger("actBoost");
        AudioManager.instance.PlayEnemySFX(AudioManager.EnemySfx.ExcelBoostStart);
    }

    public void Boost() // Start의 애니메이션 이벤트로 실행
    {
        collider.enabled = true; // 공격 판정 활성화
        animator.SetBool("onBoost", true);
        AudioManager.instance.PlayEnemySFX(AudioManager.EnemySfx.ExcelBoost);
    }

    public void BoostEnd()
    {
        collider.enabled = false; // 공격 판정 비활성화
        animator.SetBool("onBoost", false);
        AudioManager.instance.MuteEnemySFX(AudioManager.EnemySfx.ExcelBoost); // Boost의 사운드를 종료한다.
        AudioManager.instance.PlayEnemySFX(AudioManager.EnemySfx.ExcelBoostEnd);
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