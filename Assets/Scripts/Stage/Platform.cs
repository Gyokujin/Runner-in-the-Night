using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPos;

    void OnEnable()
    {
        for (int i = 0; i < spawnPos.Length; i++)
        {
            spawnPos[i].gameObject.SetActive(false);
            int spawn = Random.Range(0, 3);
            
            if (spawn == 2)
            {
                int spawnType = Random.Range(0, 3); // 0 : 장애물, 1 : 몬스터

                switch (spawnType)
                {
                    case 0:
                        spawnPos[i].gameObject.SetActive(true);
                        break;
                    case 1:
                    case 2:
                        Spawn(i, GameManager.instance.score);
                        break;
                }
            }
        }
    }

    void Spawn(int pos, int point)
    {
        GameObject spawnEnemy = null;
        int enemyKind = 0;

        if (point < 20)
            return;

        if (point < 200)
        {
            enemyKind = Random.Range(0, 2);
        }
        else if (point < 500)
        {
            enemyKind = Random.Range(0, 4);
        }
        else if (point < 1000)
        {
            enemyKind = Random.Range(0, 5);
        }

        spawnEnemy = PoolManager.instance.Get(PoolManager.PoolType.Enemy, enemyKind);
        spawnEnemy.transform.parent = transform;
        spawnEnemy.transform.position = spawnPos[pos].position;
    }
}