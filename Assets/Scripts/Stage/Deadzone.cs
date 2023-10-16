using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadzone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().Damage(true);
        }
        else if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().Die(true);
        }
        else if (collision.CompareTag("PlayerBullet") || collision.CompareTag("EnemyBullet"))
        {
            Destroy(collision.gameObject);
        }
    }
}