using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    public enum AttackType
    {
        Obstacle, Bullet, Deadzone
    }

    public AttackType attackType;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (attackType)
            {
                case AttackType.Obstacle:
                case AttackType.Bullet:
                    if (collision.gameObject.layer == 6) // 6 : 플레이어, 7 : 무적 상태
                    {
                        collision.GetComponent<PlayerController>().Damage(false);
                    }
                    break;

                case AttackType.Deadzone:
                    collision.GetComponent<PlayerController>().Damage(true);
                    break;

            }
        }
    }
}