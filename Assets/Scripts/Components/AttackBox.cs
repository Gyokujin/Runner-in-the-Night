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
        if (collision.CompareTag("Player") && collision.gameObject.layer == 6) // 6 : 플레이어, 7 : 무적 상태
        {
            bool outSide = false;

            switch (attackType)
            {
                case AttackType.Obstacle:
                    outSide = false;
                    break;
                case AttackType.Bullet:
                    outSide = false;
                    Destroy(gameObject);
                    break;
                case AttackType.Deadzone:
                    outSide = true;
                    Debug.Log("재등장");
                    break;
            }

            collision.GetComponent<PlayerController>().Damage(outSide);
        }
    }
}