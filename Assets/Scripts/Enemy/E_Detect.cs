using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Detect : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            Enemy enemy = GetComponentInParent<Enemy>();
            
            if (enemy.type == Enemy.EnemyType.Chase)
            {
                enemy.GetComponent<E_Chase>().Detect(player);
            }
            else if (enemy.type == Enemy.EnemyType.Patrol)
            {
                enemy.GetComponent<E_Patrol>().Detect(player);
            }

            gameObject.SetActive(false);
        }
    }
}