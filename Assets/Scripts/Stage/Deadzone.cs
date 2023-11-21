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
                    collision.GetComponent<PlayerController>().FallDown();
                    break;
                case "Enemy":
                case "PlayerBullet":
                case "EnemyBullet":
                case "Obstacle":
                    collision.gameObject.transform.position = Vector2.zero;
                    PoolManager.instance.Return(collision.gameObject);
                    break;
            }
        }
    }
}