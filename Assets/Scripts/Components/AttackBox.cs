using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AttackBox : MonoBehaviour
{
    public enum AttackType
    {
        Obstacle, Enemy, EnemyBullet, Deadzone
    }

    public AttackType attackType;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (attackType)
            {
                case AttackType.Obstacle:
                case AttackType.Enemy:
                    if (collision.gameObject.layer == 6)
                    {
                        collision.GetComponent<PlayerController>().Damage(false);
                    }
                    break;
                case AttackType.EnemyBullet:
                    if (collision.gameObject.layer == 6) // 6 : 플레이어, 7 : 무적 상태
                    {
                        collision.GetComponent<PlayerController>().Damage(false);
                        Destroy(gameObject);
                    }
                    break;

                case AttackType.Deadzone:
                    collision.GetComponent<PlayerController>().Damage(true);
                    break;
            }
        }
    }
}