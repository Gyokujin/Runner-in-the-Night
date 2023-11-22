using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Detect : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Enemy enemy = GetComponentInParent<Enemy>();

            switch (enemy.type)
            {
                case Enemy.EnemyType.LineMove:
                    enemy.GetComponent<E_LineMove>().Detect();
                    break;

                case Enemy.EnemyType.Chase:
                    enemy.GetComponent<E_Chase>().Detect(collision.transform.position);
                    break;

                case Enemy.EnemyType.Patrol:
                    enemy.GetComponent<E_Patrol>().Detect(collision.transform.position);
                    break;
            }
            
            gameObject.SetActive(false);
        }
    }
}