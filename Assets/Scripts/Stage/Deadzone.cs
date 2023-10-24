using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadzone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>())
        {
            string objectTag = collision.gameObject.tag;

            switch (objectTag)
            {
                case "Player":
                    collision.GetComponent<PlayerController>().Damage(true);
                    break;
                case "Enemy":
                    collision.GetComponent<Enemy>().Die(true);
                    break;
                case "PlayerBullet":
                case "EnemyBullet":
                case "Obstacle":
                    collision.gameObject.SetActive(false);
                    break;
            }
        }

        //if (collision.CompareTag("Player"))
        //{
            
        //}
        //else if (collision.CompareTag("Enemy"))
        //{
            
        //}
        //else if (collision.CompareTag("PlayerBullet") || collision.CompareTag("EnemyBullet"))
        //{

        //}
    }
}