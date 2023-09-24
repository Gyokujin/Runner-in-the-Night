using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    public enum AttackType
    {
        Obstacle, Bullet
    }

    public AttackType attackType;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.gameObject.layer == 6) // 6 : 플레이어, 7 : 무적 상태
        {
            collision.GetComponent<PlayerController>().Damage();

            if (attackType == AttackType.Bullet) // 만약 총알일 경우 플레이어에게 데미지를 주고 파괴한다
            {
                Destroy(gameObject);
            }
        }
    }
}