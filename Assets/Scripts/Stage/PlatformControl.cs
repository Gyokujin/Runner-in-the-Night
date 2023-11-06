using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControl : MonoBehaviour
{
    private GameObject[] platforms;
    
    [Header("Transfer")]
    private int moveIndex = 0;
    [SerializeField]
    private float minXPos;
    private float lastYPos;
    [SerializeField]
    private float transferX;
    [SerializeField]
    private float transferYMin;
    [SerializeField]
    private float transferYMax;
    [SerializeField]
    private float transferYLimit;

    [Header("Spawn")]
    [SerializeField]
    private float[] spawnPosX;
    private SpawnData spawnData;

    void Awake()
    {
        Init();
        spawnData = GetComponent<SpawnData>();
    }

    void Init()
    {
        platforms = new GameObject[transform.childCount];

        for (int i = 0; i < platforms.Length; i++)
        {
            platforms[i] = transform.GetChild(i).gameObject;
        }
    }

    void Update()
    {
        if (platforms[moveIndex].transform.position.x < minXPos)
        {
            PlatformTransfer(moveIndex);
        }
    }

    void PlatformTransfer(int platformIndex)
    {
        float transferY = Random.Range(Mathf.Max(lastYPos - transferYLimit, transferYMin), Mathf.Min(lastYPos  + transferYLimit, transferYMax));
        lastYPos = transferY;
        platforms[platformIndex].transform.position = new Vector2(transferX, transferY);
        PlatformSetting(platformIndex);
        moveIndex = (moveIndex + 1) % (platforms.Length);
    }

    void PlatformSetting(int platformIndex)
    {
        for (int i = 0; i < spawnPosX.Length; i++)
        {
            GameObject spawnObject = null;
            int spawnType = Random.Range(0, 6); // 0 : 장애물, 1, 2 : 몬스터

            switch (spawnType)
            {
                case 0:
                    spawnObject = PoolManager.instance.Get(PoolManager.PoolType.Obstacle, 0);
                    spawnObject.transform.parent = platforms[platformIndex].transform;
                    spawnObject.transform.localPosition = new Vector2(spawnPosX[i], spawnData.spikePosY);
                    break;

                case 1:
                case 2:
                    int enemyKind = spawnData.SelectEnemy();
                    spawnObject = PoolManager.instance.Get(PoolManager.PoolType.Enemy, enemyKind);
                    spawnObject.transform.parent = platforms[platformIndex].transform;
                    spawnObject.transform.localPosition = new Vector2(spawnPosX[i], spawnData.enemyPosY[enemyKind]);
                    break;
            }
        }
    }
}