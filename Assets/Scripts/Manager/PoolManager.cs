using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance = null;

    public enum PoolType
    {
        Bullet,
        Obstacle,
        Enemy
    }

    [Header("Bullet")]
    public GameObject[] bullets;
    private List<GameObject>[] bulletPool;

    [Header("Obstacle")]
    public GameObject obstacle;
    private List<GameObject> obstaclePool;

    [Header("Enemy")]
    public GameObject[] enemies;
    private List<GameObject>[] enemyPool;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        bulletPool = new List<GameObject>[bullets.Length];
        obstaclePool = new List<GameObject>();
        enemyPool = new List<GameObject>[enemies.Length];

        for (int i = 0; i < bulletPool.Length; i++)
        {
            bulletPool[i] = new List<GameObject>();
        }

        for (int j = 0; j < enemies.Length; j++)
        {
            enemyPool[j] = new List<GameObject>();
        }
    }

    public GameObject Get(PoolType type, int index)
    {
        GameObject select = null;

        if (type == PoolType.Bullet)
        {
            foreach (GameObject bullet in bulletPool[index])
            {
                if (!bullet.activeSelf)
                {
                    select = bullet;
                    select.SetActive(true);
                    break;
                }
            }

            if (!select)
            {
                select = Instantiate(bullets[index], transform);
                bulletPool[index].Add(select);
            }
        }
        else if (type == PoolType.Obstacle)
        {
            foreach (GameObject obstacle in obstaclePool)
            {
                if (!obstacle.activeSelf)
                {
                    select = obstacle;
                    select.SetActive(true);
                    break;
                }
            }

            if (!select)
            {
                select = Instantiate(obstacle, transform);
                obstaclePool.Add(select);
            }
        }
        else if (type == PoolType.Enemy)
        {
            foreach (GameObject enemy in enemyPool[index])
            {
                if (!enemy.activeSelf)
                {
                    select = enemy;
                    select.SetActive(true);
                    break;
                }
            }

            if (!select)
            {
                select = Instantiate(enemies[index], transform);
                enemyPool[index].Add(select);
            }

            select.GetComponent<Enemy>().Init();
        }
        
        return select;
    }

    public void Return(GameObject poolObject)
    {
        poolObject.transform.parent = null;
        poolObject.transform.parent = gameObject.transform;
        poolObject.SetActive(false);
    }
}